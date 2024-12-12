using Courses_App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Courses_App.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private const string CategoriesEndpoint = "api/categories";

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainPage.AuthToken);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/" + CategoriesEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Category>>(content);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync(CategoriesEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category), System.Text.Encoding.UTF8, "application/json");

            // Gantilah URL di sini jika perlu
            var response = await _httpClient.PostAsync("https://actbackendseervices.azurewebsites.net/" + CategoriesEndpoint, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/categories/{category.CategoryId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync("https://actbackendseervices.azurewebsites.net/" + $"api/categories/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error deleting category: {response.StatusCode} - {errorMsg}");
            }
        }
    }
}
