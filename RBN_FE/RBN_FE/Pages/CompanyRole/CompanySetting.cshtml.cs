using BusinessObject.DTO;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BusinessObject.DTO.RequestDto;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using BusinessObject.Dto.RequestDto;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanySettingModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly Cloudinary _cloudinary;

        public CompanySettingModel(IHttpClientFactory clientFactory, IConfiguration configuration, Cloudinary cloudinary)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _cloudinary = cloudinary;
        }

        private static string APIPort = "http://localhost:5250/api/";

        [BindProperty]
        public CompanyDTO Company { get; set; }

        [BindProperty]
        [Display(Name = "Company Avatar")]
        public IFormFile AvatarFile { get; set; }

        [BindProperty]
        public IFormFile EventImage { get; set; }

        // Thuộc tính để hiển thị thông báo trạng thái
        [TempData]
        public string StatusMessage { get; set; }

        private async Task<string> UploadAvatar()
        {
            if (AvatarFile == null || AvatarFile.Length == 0)
                return null;

            using (var stream = AvatarFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(AvatarFile.FileName, stream),
                    Transformation = new Transformation().Width(300).Height(300).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task OnGetAsync(int companyId)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var userIdStr = HttpContext.Session.GetString("UserId");

                    if (int.TryParse(userIdStr, out int userId))
                    {
                        // Fetch company info using companyId or userId
                        using (var response = await httpClient.GetAsync(APIPort + "Company/user/" + userId))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                Company = JsonConvert.DeserializeObject<CompanyDTO>(content);
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tải lên ảnh đại diện nếu có
            string avatarUrl = await UploadAvatar();

            // Cập nhật URL avatar nếu đã tải lên
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                Company.Avatar = avatarUrl;
            }

            // Tạo đối tượng cập nhật
            var updatedCompany = new CompanyDTO
            {
                Id = Company.Id,
                Name = Company.Name,
                Description = Company.Description,
                Address = Company.Address,
                Phone = Company.Phone,
                Avatar = Company.Avatar,
                TaxCode = Company.TaxCode,
                Status = Company.Status,
                UserId = Company.UserId
            };

            // Chuẩn bị nội dung JSON để gửi
            var jsonContent = JsonConvert.SerializeObject(updatedCompany);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Lấy companyId từ URL hoặc từ đối tượng Company
                    int companyId = Company.Id;

                    // Gửi yêu cầu PUT đến API
                    var response = await httpClient.PutAsync($"{APIPort}Company/companyid?companyid={companyId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        StatusMessage = "Cập nhật thông tin công ty thành công.";
                        return RedirectToPage(); // Tải lại trang để hiển thị thông tin cập nhật
                    }
                    else
                    {
                        StatusMessage = "Cập nhật thông tin công ty thất bại.";
                        // Bạn có thể thêm thêm xử lý lỗi chi tiết dựa trên response
                    }
                }
                catch (HttpRequestException ex)
                {
                    StatusMessage = $"Lỗi khi kết nối đến API: {ex.Message}";
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Đã xảy ra lỗi: {ex.Message}";
                }
            }

            return Page();
        }
    }
}
