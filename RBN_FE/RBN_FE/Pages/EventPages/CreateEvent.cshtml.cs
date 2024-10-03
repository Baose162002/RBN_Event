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
        public void OnGet()
        {
            Input = new CreateEventDto();
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
                    ModelState.AddModelError(string.Empty, "You are not authenticated. Please log in.");
                    return Page();
                }

                // Lấy CompanyId từ Role trong session
                // Giả sử bạn có một phương thức để lấy CompanyId từ Role
                int companyId = GetCompanyIdFromRole(HttpContext.Session.GetString("UserRole"));

                Input.CompanyId = companyId; // Thay vì dùng UserRole





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
                        ModelState.AddModelError("EventImage", "Failed to upload the image. Please try again.");
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
                    TempData["SuccessMessage"] = "Batch created successfully!";
                    return RedirectToPage("/EventPages/EventIndex");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error creating event. Status code: {errorContent}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating event");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return Page();
            }
        }
        private int GetCompanyIdFromRole(string role)
        {
            if (role == "Admin")
            {
                return 1;
            }
            else if (role == "Company")
            {
                return 3;
            }

            _logger.LogError($"Invalid role: {role}");
            throw new ArgumentException($"Invalid role: {role}");
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
    }
}
