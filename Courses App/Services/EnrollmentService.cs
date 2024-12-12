using Courses_App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Courses_App.Services
{
    public class EnrollmentService
    {
        private readonly HttpClient _httpClient;
        private const string EnrollmentEndpoint = "api/enrollments";

        public EnrollmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainPage.AuthToken);
        }
        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            var response = await _httpClient.GetAsync(EnrollmentEndpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Enrollment>>();
        }

        public async Task<Enrollment> GetEnrollmentByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/enrollments/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Enrollment>(content);
        }
        public async Task CreateEnrollmentAsync(Enrollment enrollment)
        {
            var content = new StringContent(JsonSerializer.Serialize(enrollment), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/enrollments", content);
            response.EnsureSuccessStatusCode();
        }



    }
}
