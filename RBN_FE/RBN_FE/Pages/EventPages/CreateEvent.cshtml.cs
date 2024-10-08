using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Text.Json;
using System.Text;
using BusinessObject.DTO.RequestDto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BusinessObject.DTO;
using System.Net.Http.Headers;
using BusinessObject;
using BusinessObject.Enum;

namespace RBN_FE.Pages.EventPages
{
    public class CreateEventModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateEventModel> _logger;

        public CreateEventModel(HttpClient httpClient, IConfiguration configuration, ILogger<CreateEventModel> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public CreateEventDto Input { get; set; }
        [BindProperty]
        public IFormFile EventImage { get; set; }
        public List<CompanyDTO> Companies { get; set; } = new List<CompanyDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            Input = new CreateEventDto();


            var token = HttpContext.Session.GetString("JWTToken");
            var role = HttpContext.Session.GetString("UserRole");


            if (role == "Admin") // Assuming you store the role as a string
            {
                Companies = await GetCompaniesAsync(token); // Fetch companies
            }

            // Additional logic for loading the event details, if applicable
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("EventImage");

            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("JWT Token is missing from session");
                    ModelState.AddModelError(string.Empty, "Bạn chưa login. Vui lòng login vào");
                    return Page();
                }

                var userRole = HttpContext.Session.GetString("UserRole");

                if (userRole == "Admin")
                {
                    // If the user is an admin, the CompanyId should be provided by the dropdown
                    // Validate that the CompanyId was selected
                    if (Input.CompanyId <= 0) // or check for null if nullable
                    {
                        ModelState.AddModelError(string.Empty, "Hãy chọn company");
                        return Page();
                    }
                }
                else if (userRole == "Company")
                {
                    // Logic for fetching CompanyId for Company role remains unchanged
                    var userIdString = HttpContext.Session.GetString("UserId");
                    if (string.IsNullOrEmpty(userIdString))
                    {
                        ModelState.AddModelError(string.Empty, "User ID not found in session.");
                        return Page();
                    }

                    if (!int.TryParse(userIdString, out int userId))
                    {
                        ModelState.AddModelError(string.Empty, "Invalid User ID.");
                        return Page();
                    }

                    var company = await GetCompanyByUserIdAsync(userIdString);
                    if (company == null)
                    {
                        ModelState.AddModelError(string.Empty, "Không tìm thấy company của bạn");
                        return Page();
                    }
                    Input.CompanyId = company.Id; // Set the Company ID for Company role
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid user role.");
                    return Page();
                }




                // Xử lý upload ảnh nếu có
                if (EventImage != null)
                {
                    var imageId = await UploadImageAsync(EventImage);
                    if (imageId.HasValue)
                    {
                        Input.EventImgId = imageId.Value;
                    }
                    else
                    {
                        ModelState.AddModelError("EventImage", "Upload ảnh bị lỗi. Vui lòng thử lại");
                        return Page();
                    }
                }

                // Phần còn lại của mã xử lý POST
                var json = JsonSerializer.Serialize(Input);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var apiUrl = $"{_configuration["ApiSettings:BaseUrl"]}/event";

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/EventPages/EventIndex");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Cập nhật thông tin bị lỗi: {errorContent}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating event");
                ModelState.AddModelError(string.Empty, "Có trục gì đó. Vui lòng thử lại");
                return Page();
            }
        }
       


        public async Task<int?> UploadImageAsync(IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            using var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            content.Add(new StreamContent(new MemoryStream(stream.ToArray())), "file", image.FileName);

            var response = await _httpClient.PostAsync($"{_configuration["ApiSettings:BaseUrl"]}/event/upload", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Ghi lại nội dung phản hồi để kiểm tra
            _logger.LogInformation("Upload response content: " + responseContent);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    using (var document = JsonDocument.Parse(responseContent))
                    {
                        if (document.RootElement.TryGetProperty("imageId", out var imageIdProperty))
                        {
                            return imageIdProperty.GetInt32();
                        }
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"JSON deserialization failed: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError($"Upload failed with status code: {response.StatusCode}. Response: {responseContent}");
            }

            return null; // Trả về null nếu có lỗi
        }
        private async Task<List<CompanyDTO>> GetCompaniesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var apiUrl = $"{_configuration["ApiSettings:BaseUrl"]}/company"; // Adjust API URL accordingly

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CompanyDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new List<CompanyDTO>(); // Return empty list on failure
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
            var apiUrl = $"{_configuration["ApiSettings:BaseUrl"]}/Company/user/{userId}";

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
                _logger.LogError($"Error fetching company information. Status code: {response.StatusCode}, Content: {errorContent}");
                return null;



            }
        }

    }

}
