namespace Courses_App.Pages;
using Courses_App.Models;
using Courses_App.Services;


public partial class AddCategory : ContentPage
{
    private readonly CategoryService _categoryService;

    public AddCategory(CategoryService categoryService)
    {
        InitializeComponent();
        _categoryService = categoryService;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var newCategory = new Category
        {
            Name = CategoryNameEntry.Text,
            Description = CategoryDescriptionEntry.Text
        };

        try
        {
            await _categoryService.CreateCategoryAsync(newCategory);
            await DisplayAlert("Success", "Category added successfully!", "OK");
            await Navigation.PopAsync(); // Navigate back after saving
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}