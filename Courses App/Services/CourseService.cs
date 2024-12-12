using Courses_App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Courses_App.Services
{
    public class CourseService
    {
        private readonly HttpClient _httpClient;
        private const string CoursesEndpoint = "api/Courses";


        public CourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainPage.AuthToken);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var response = await _httpClient.GetAsync(CoursesEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Course>>();
        }
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/courses/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Course>(content);
        }

        public async Task CreateCourseAsync(Course course)
        {
            var content = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/courses", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var content = new StringContent(JsonSerializer.Serialize(course), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/courses/{course.CourseId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/courses/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<bool> AddCourseAsync(Course course)
        {
            var response = await _httpClient.PostAsJsonAsync("api/courses", course);
            return response.IsSuccessStatusCode;
        }
    }
}
