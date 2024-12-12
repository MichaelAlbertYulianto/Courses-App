using Courses_App.Models;
using Courses_App.Services;


namespace Courses_App.Pages;

public partial class EditCourse : ContentPage
{
    private readonly CourseWithSelected _course;
    private readonly CategoryService _categoryService;

    public EditCourse(CourseWithSelected course, CategoryService categoryService)
    {
        InitializeComponent();
        _course = course;
        _categoryService = categoryService;

        LoadCourseData();
        LoadCategories();
    }
    private async Task LoadCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        CourseCategoryPicker.ItemsSource = categories.ToList(); // Convert to List
        CourseCategoryPicker.ItemDisplayBinding = new Binding("Name");

        if (_course.Category != null)
        {
            CourseCategoryPicker.SelectedItem = categories.FirstOrDefault(c => c.CategoryId == _course.Category.CategoryId);
        }
    }

    private void LoadCourseData()
    {
        CourseNameEntry.Text = _course.Name;
        CourseDescriptionEditor.Text = _course.Description;
        CourseImageNameEntry.Text = _course.ImageName;
        CourseDurationEntry.Text = _course.Duration.ToString();
    }



    private async void OnSaveChanges(object sender, EventArgs e)
    {
        _course.Name = CourseNameEntry.Text;
        _course.Description = CourseDescriptionEditor.Text;
        _course.ImageName = CourseImageNameEntry.Text;
        _course.Duration = int.TryParse(CourseDurationEntry.Text, out var duration) ? duration : _course.Duration;
        _course.Category = (Category)CourseCategoryPicker.SelectedItem;

        // Save changes back to the data source or API here if needed
        await Navigation.PopAsync();
    }
}