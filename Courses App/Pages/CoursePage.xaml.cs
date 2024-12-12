using Courses_App.Models;
using Courses_App.Services;
using System.Collections.ObjectModel;

namespace Courses_App.Pages;

public partial class CoursePage : ContentPage
{
    private readonly CategoryService _categoryService;
    private readonly CourseService _courseService;
    private ObservableCollection<CourseWithSelected> _courses;

    public CoursePage(CourseService courseService, CategoryService categoryService)
    {
        InitializeComponent();

        _courseService = courseService; // Simpan instance CourseService
        _categoryService = categoryService; // Simpan instance CategoryService
        _courses = new ObservableCollection<CourseWithSelected>();
        CoursesListView.ItemsSource = _courses;

        LoadCourses(); // Load courses saat page diinisialisasi
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourses();
    }

    private async Task LoadCourses()
    {
        try
        {
            var courses = await _courseService.GetAllCoursesAsync();
            _courses.Clear();
            foreach (var course in courses)
            {
                // Create a CourseWithSelected instance
                var courseWithSelection = new CourseWithSelected
                {
                    CourseId = course.CourseId,
                    Name = course.Name,
                    ImageName = course.ImageName,
                    Duration = course.Duration,
                    Description = course.Description,
                    Category = course.Category, // Assigning the associated Category
                    IsSelected = false // Initialize IsSelected to false
                };
                _courses.Add(courseWithSelection);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
        }
    }

    private async void OnUpdateSelectedCourses(object sender, EventArgs e)
    {
        // Cari kursus yang dipilih (IsSelected == true)
        var selectedCourses = _courses.Where(c => c.IsSelected).ToList();

        if (selectedCourses.Count == 1)
        {
            var selectedCourse = selectedCourses.First(); // Ambil kursus pertama yang dipilih

            // Arahkan ke halaman EditCourse dengan kursus yang dipilih dan kategori
            await Navigation.PushAsync(new EditCourse(selectedCourse, _categoryService));
        }
        else if (selectedCourses.Count > 1)
        {
            await DisplayAlert("Error", "Please select only one course to update.", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Please select a course to update.", "OK");
        }
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCourse());
    }

    private async void OnRefreshList(object sender, EventArgs e)
    {
        await LoadCourses();
    }

    private void OnCourseSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedCourse = e.SelectedItem as CourseWithSelected;

        UpdateSelectedCourseButton.IsEnabled = _courses.Any(c => c.IsSelected);
        if (selectedCourse != null)
        {
            // You can enable the Update and Delete buttons here
            UpdateSelectedCourseButton.IsEnabled = true; // Example of enabling update button
        }
        else
        {
            // Handle deselection if necessary
            UpdateSelectedCourseButton.IsEnabled = false; // Disable if no selection
        }
    }

    private async void OnDeleteSelectedCourses(object sender, EventArgs e)
    {
        var selectedCourses = _courses.Where(c => c.IsSelected).ToList();

        if (selectedCourses.Count > 0)
        {
            bool confirm = await DisplayAlert("Confirm Delete",
                $"Are you sure you want to delete the selected courses?",
                "Yes",
                "No");
            if (confirm)
            {
                try
                {
                    foreach (var course in selectedCourses)
                    {
                        await _courseService.DeleteCourseAsync(course.CourseId);
                        _courses.Remove(course);
                    }
                    await DisplayAlert("Success", "Selected courses deleted successfully!", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete courses: {ex.Message}", "OK");
                }
            }
        }
        else
        {
            await DisplayAlert("Warning", "Please select at least one course to delete.", "OK");
        }
    }
}