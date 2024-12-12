using Courses_App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Courses_App.Services
{
    public class InstructorService
    {
        private readonly HttpClient _httpClient;
        private const string InstructorEndpoint = "api/instructors";

        public InstructorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainPage.AuthToken);
        }
        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            var response = await _httpClient.GetAsync(InstructorEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Instructor>>();
        }

        public async Task<Instructor> GetInstructorByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync("api/instructors");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var instructors = JsonSerializer.Deserialize<IEnumerable<Instructor>>(content);
            return instructors?.FirstOrDefault(i => i.instructorId == id);
        }

    }
}
