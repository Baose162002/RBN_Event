using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net;
using BusinessObject.DTO.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;

namespace RBN_FE.Pages.EventPages
{
    public class EventIndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public EventIndexModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseApiUrl = configuration["ApiSettings:BaseUrl"];
        }

        public List<EventDTO> Events { get; set; } = new List<EventDTO>();
        public string SearchTerm { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            // Get token from session
            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login&Out/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Prepare the API request URL
            var requestUrl = $"{_baseApiUrl}/event?searchTerm={Uri.EscapeDataString(searchTerm)}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<EventDTO>>();

                if (pagedResult != null)
                {
                    Events = pagedResult.Data;
                    TotalPages = pagedResult.TotalPages; // Total pages returned from the API
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Token không hợp lệ hoặc hết hạn.");
                return RedirectToPage("/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi tải danh sách sự kiện: {response.ReasonPhrase}");
            }

            SearchTerm = searchTerm; // Set search term for the view
            PageNumber = pageNumber; // Set current page number

            return Page();
        }
    }
}
