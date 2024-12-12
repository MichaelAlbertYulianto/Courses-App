using Courses_App.Models;
using Courses_App.Services;

namespace Courses_App.Pages;

public partial class AddCourse : ContentPage
{
    private readonly CourseService _courseService;
    private readonly CategoryService _categoryService;
    private List<Category> _categories;

    public AddCourse()
    {
        InitializeComponent();
        _courseService = new CourseService(new HttpClient());
        _categoryService = new CategoryService(new HttpClient());
        LoadCategories(); // Load categories when the page is initialized
    }

    private async Task LoadCategories()
    {
        try
        {
            _categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
            CourseCategoryPicker.ItemsSource = _categories;
            CourseCategoryPicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        var selectedCategory = CourseCategoryPicker.SelectedItem as Category;

        // Gather the data from the input fields
        var course = new Course
        {
            Name = CourseNameEntry.Text,
            Description = CourseDescriptionEditor.Text,
            ImageName = CourseImageNameEntry.Text,
            Duration = int.TryParse(CourseDurationEntry.Text, out var duration) ? duration : 0,
            Category = selectedCategory // Set the selected category
        };

        try
        {
            // Send the course object to the API to add it
            var result = await _courseService.AddCourseAsync(course);
            if (result)
            {
                await DisplayAlert("Success", "Course added successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to add course.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add course: {ex.Message}", "OK");
        }
    }

}