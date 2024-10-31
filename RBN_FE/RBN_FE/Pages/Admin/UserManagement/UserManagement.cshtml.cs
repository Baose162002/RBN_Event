using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessObject.Dto.ResponseDto;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;

namespace RBN_FE.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private static readonly string APIPort = "https://rbnapi20241031155156.azurewebsites.net/api/";
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }  
        public List<UserResponseDto> Users { get; set; }

        public UserManagementModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<IActionResult> OnPostChangeStatusAsync()
        {
            if (UserId == null)
            {
                ModelState.AddModelError("", "User không hợp lệ.");
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var apiUrl = $"{APIPort}User/inactive/{UserId.Value}";
                    using (var response = await httpClient.PutAsync(apiUrl, null))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Trạng thái người dùng đã được cập nhật thành công.";
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không thể thay đổi trạng thái người dùng.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi gửi yêu cầu: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi không mong đợi: {ex.Message}");
                }
            }

            await OnGetAsync();

            return RedirectToPage("/Admin/UserManagement/UserManagement");
        }
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/User");

            if (response.IsSuccessStatusCode)
            {
                Users = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
            }
            else
            {
                // Handle error - maybe set an error message to display on the page
                Users = new List<UserResponseDto>();
            }
        }
    }
}