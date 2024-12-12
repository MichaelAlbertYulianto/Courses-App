namespace Courses_App.Pages;
using Courses_App.Models;
using Courses_App.Services;
using System;

public partial class EditCategory : ContentPage
{
    private readonly CategoryService _categoryService;
    private readonly CategoryWithSelection _category;

    public EditCategory(CategoryWithSelection category, CategoryService categoryService)
    {
        InitializeComponent();
        _categoryService = categoryService;
        _category = category;

        // Initialize fields with data from _category
        CategoryNameEntry.Text = _category.Name;
        CategoryDescriptionEditor.Text = _category.Description;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Logic for saving the category
        _category.Name = CategoryNameEntry.Text;
        _category.Description = CategoryDescriptionEditor.Text;

        try
        {
            await _categoryService.UpdateCategoryAsync(_category); // Assuming you have this method
            await DisplayAlert("Success", "Category updated successfully!", "OK");
            await Navigation.PopAsync(); // Return to the previous page
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to update category: {ex.Message}", "OK");
        }
    }
}