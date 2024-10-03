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
using BusinessObject.DTO.ResponseDto;
using Microsoft.EntityFrameworkCore;

namespace RBN_FE.Pages.EventPages
{
    public class UpdateEventModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UpdateEventModel> _logger;

        public UpdateEventModel(HttpClient httpClient, IConfiguration configuration, ILogger<UpdateEventModel> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public UpdateEventDto Input { get; set; }

        [BindProperty]
        public IFormFile EventImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("JWT Token is missing from session");
                ModelState.AddModelError(string.Empty, "You are not authenticated. Please log in.");
                return RedirectToPage("/Account/Login");
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
                    Input = JsonSerializer.Deserialize<UpdateEventDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

        public async Task<IActionResult> OnPostAsync(int id) // Pass eventId separately
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
                    ModelState.AddModelError(string.Empty, "You are not authenticated. Please log in.");
                    return Page();
                }
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
                
                var apiUrl = $"{_configuration["ApiSettings:BaseUrl"]}/event/{id}"; // Pass eventId separately in the URL

                // Serialize the DTO
                var json = JsonSerializer.Serialize(Input);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Event updated successfully!";
                    return RedirectToPage("/EventPages/EventIndex");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error updating event. Status code: {errorContent}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating event");
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

            return null;
        }
    }
}
