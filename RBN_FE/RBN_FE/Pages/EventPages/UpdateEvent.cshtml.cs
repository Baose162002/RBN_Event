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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                return RedirectToPage("/LogIn&Out/Login");
            }

            // Extract the role from JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Store the role in ViewData for use in Razor Page
            ViewData["UserRole"] = roleClaim;

            try
            {
                // Fetch event data
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
           



                var existingEvent = await GetEventByIdAsync(id); // Khai báo existingEvent và lấy dữ liệu sự kiện
                if (existingEvent == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load event data.");
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
                        ModelState.AddModelError("EventImage", "Failed to upload the image. Please try again.");
                        return Page();
                    }
                }
                else
                {
                    // Giữ lại EventImgId nếu không có ảnh mới
                    Input.EventImgId = existingEvent.EventImg.Id;
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

        private async Task<EventDTO> GetEventByIdAsync(int eventId)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("JWT Token is missing from session");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var apiUrl = $"{_configuration["ApiSettings:BaseUrl"]}/event/{eventId}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var eventDto = JsonSerializer.Deserialize<EventDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Manually map EventImgId if it's available in the response
                if (eventDto.EventImg == null)
                {
                    eventDto.Id = eventDto.EventImg.Id;  // Extract EventImgId from the eventImg object
                }

                return eventDto;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error fetching event by ID. Status code: {response.StatusCode}, Content: {errorContent}");
                return null;
            }
        }


    }
}
