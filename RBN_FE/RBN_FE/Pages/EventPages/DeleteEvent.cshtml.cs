using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net;
using BusinessObject.DTO;
using System.Text.Json;

namespace RBN_FE.Pages.EventPages
{
    public class DeleteEventModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public DeleteEventModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseApiUrl = configuration["ApiSettings:BaseUrl"];
        }

        public EventDTO Event { get; set; }
        public int EventId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            EventId = id;

            // Lấy token từ session
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login&Out/Login");
            }

            // Thêm token vào header của HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Gọi API để lấy sự kiện theo ID
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/event/{id}");

            if (response.IsSuccessStatusCode)
            {
                Event = await response.Content.ReadFromJsonAsync<EventDTO>();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return RedirectToPage("/EventPages/EventIndex");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi tải sự kiện: {response.ReasonPhrase}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Xóa sự kiện thông qua API
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login&Out/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{_baseApiUrl}/event/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/EventPages/EventIndex");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi xóa sự kiện: {response.ReasonPhrase}");
                return Page();
            }
        }
    }
}
