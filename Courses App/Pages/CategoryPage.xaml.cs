using Courses_App.Models;
using Courses_App.Services;
using System.Collections.ObjectModel;

namespace Courses_App.Pages;

public partial class CategoryPage : ContentPage
{
    private readonly CategoryService _categoryService;
    private ObservableCollection<CategoryWithSelection> _categories;

    public CategoryPage(CategoryService categoryService)
    {
        InitializeComponent();
        _categoryService = categoryService;
        _categories = new ObservableCollection<CategoryWithSelection>();
        CategoriesListView.ItemsSource = _categories;

        LoadCategories(); // Load categories when page is initialized
    }
    private async Task LoadCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync(); // Assuming this method exists
            _categories.Clear(); // Clear existing categories
            foreach (var category in categories)
            {
                // Wrap category in your CategoryWithSelection model
                _categories.Add(new CategoryWithSelection
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Description = category.Description
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    private async void OnUpdateSelectedCategory(object sender, EventArgs e)
    {
        var selectedCategory = _categories.FirstOrDefault(c => c.IsSelected);

        if (selectedCategory != null)
        {
            // Navigate to EditCategory page, passing the selected category and categoryService
            await Navigation.PushAsync(new EditCategory(selectedCategory, _categoryService));
        }
        else
        {
            await DisplayAlert("Error", "Please select a category to update.", "OK");
        }
    }

    private void OnCategorySelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedCategory = _categories.FirstOrDefault(c => c.IsSelected);

        if (selectedCategory != null)
        {
            // You can perform actions with the selected category here
            // For example, you might enable the Update and Delete buttons
            UpdateSelectedCategoryButton.IsEnabled = true; // Example of enabling update button
                                                           // Add additional logic if needed
        }
        else
        {
            // Handle deselection if necessary
            UpdateSelectedCategoryButton.IsEnabled = false; // Disable if no selection
        }
    }
    private async void OnAddCategory(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCategory(_categoryService)); // Use the instance of CategoryService
    }
    private async void OnLoadCategory(object sender, EventArgs e)
    {
        await LoadCategories(); // Refresh the list
    }
    private async void OnDeleteSelectedCategories(object sender, EventArgs e)
    {
        // Get selected categories
        var selectedCategories = _categories.Where(c => c.IsSelected).ToList();

        if (selectedCategories.Any())
        {
            // Log the IDs of categories to be deleted
            foreach (var category in selectedCategories)
            {
                Console.WriteLine($"Attempting to delete category with ID: {category.CategoryId}");
            }

            try
            {
                foreach (var category in selectedCategories)
                {
                    await _categoryService.DeleteCategoryAsync(category.CategoryId); // Call your delete method
                    _categories.Remove(category); // Remove from the collection after deletion
                }
                await DisplayAlert("Success", "Selected categories deleted successfully.", "OK");
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"Error deleting categories: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Warning", "No categories selected for deletion.", "OK");
        }
    }
}