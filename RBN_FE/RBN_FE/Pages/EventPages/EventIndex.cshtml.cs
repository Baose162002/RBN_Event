using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net;
using BusinessObject.DTO.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using System.Text.Json;

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
            // Get token, UserId, and UserRole from session
            var token = HttpContext.Session.GetString("JWTToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login&Out/Login");
            }

            if (string.IsNullOrEmpty(userRole))
            {
                ModelState.AddModelError(string.Empty, "User role not found in session.");
                return Page(); // Or handle as needed
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Prepare the API request URL based on user role
            string requestUrl;
            var searchTermValue = searchTerm ?? string.Empty; // If searchTerm is null, use an empty string

            if (userRole == "Company")
            {
                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError(string.Empty, "User ID not found in session.");
                    return Page(); // Handle if the user is Company but UserId is missing
                }
                var company = await GetCompanyByUserIdAsync(userId);
                if (company == null)
                {
                    ModelState.AddModelError(string.Empty, "Company not found for the user.");
                    return Page();
                }
                var companyId = company.Id;
                // If user role is Company, use Company-specific API endpoint
                requestUrl = $"{_baseApiUrl}/event/company/{Uri.EscapeDataString(companyId.ToString())}?searchTerm ={Uri.EscapeDataString(searchTermValue)}&pageNumber={pageNumber}&pageSize={pageSize}";
            }
            else if (userRole == "Admin")
            {
                // If user role is Admin, get all events
                requestUrl = $"{_baseApiUrl}/event?searchTerm={Uri.EscapeDataString(searchTermValue)}&pageNumber={pageNumber}&pageSize={pageSize}";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid user role.");
                return Page();
            }

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
        private async Task<CompanyDTO> GetCompanyByUserIdAsync(string userId)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("JWT Token is missing from session");
            }

            // Thiết lập Header Authorization với JWT Token
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Tạo URL cho API để lấy thông tin công ty
            var apiUrl = $"{_baseApiUrl}/Company/user/{userId}";

            // Gọi API để lấy thông tin công ty
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var company = JsonSerializer.Deserialize<CompanyDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return company;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return null;



            }
        }
    }
}
