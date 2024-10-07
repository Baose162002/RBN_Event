using BusinessObject.DTO.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using BusinessObject.DTO.ResponseDto;
using BusinessObject.DTO;

namespace RBN_FE.Pages.EventPages
{
    public class EventDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventDetailModel> _logger;

        public EventDetailModel(HttpClient httpClient, IConfiguration configuration, ILogger<EventDetailModel> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public EventDTO Input { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("JWT Token is missing from session");
                ModelState.AddModelError(string.Empty, "You are not authenticated. Please log in.");
                return RedirectToPage("/LogIn&Out/Login");
            }

            try
            {
                // Gọi API để lấy thông tin event hiện tại
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var apiUrl = $"{_configuration["ApiSettings:BaseUrl"]}/event/{id}";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Input = JsonSerializer.Deserialize<EventDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to load event data.");
                    return RedirectToPage("/EventPages/EventIndex");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while loading event");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return RedirectToPage("/EventPages/EventIndex");
            }
        }

    }
}
