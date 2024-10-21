using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace RBN_FE.Pages.LogIn_Out
{
    public class CompanySignupModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly Cloudinary _cloudinary;

        public CompanySignupModel(IHttpClientFactory clientFactory, IConfiguration configuration, Cloudinary cloudinary)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _cloudinary = cloudinary;
        }

        [BindProperty]
        public CreateCompanyDto CreateCompanyDto { get; set; }

        [BindProperty]
        [Display(Name = "Company Avatar")]
        public IFormFile AvatarFile { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            CreateCompanyDto = new CreateCompanyDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                // Upload Avatar (if provided)
                string avatarUrl = null;
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    avatarUrl = await UploadAvatar();
                }

                // Create Company
                var companyCreated = await CreateCompany(avatarUrl);
                if (companyCreated)
                {
                    return new JsonResult(new { success = true, message = "Đăng ký công ty thành công! Vui lòng kiểm tra email để nhận tài khoản và mật khẩu" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Không thể tạo công ty. Vui lòng thử lại." });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

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

        private async Task<bool> CreateCompany(string avatarUrl)
        {
            var client = _clientFactory.CreateClient();
            var apiUrl = _configuration["ApiSettings:BaseUrl"] + "/User/create-companyFE";

            CreateCompanyDto.Avatar = avatarUrl;

            var content = new StringContent(JsonConvert.SerializeObject(CreateCompanyDto), Encoding.UTF8, "application/json");

            Console.WriteLine($"Sending request to {apiUrl}");
            Console.WriteLine($"Request content: {await content.ReadAsStringAsync()}");

            var response = await client.PostAsync(apiUrl, content);

            Console.WriteLine($"Response status: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    try
                    {
                        var errorDetails = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent);
                        var errors = errorDetails.SelectMany(kvp => kvp.Value).ToList();
                        throw new Exception($"Failed to create company. Validation errors: {string.Join(", ", errors)}");
                    }
                    catch (JsonException)
                    {
                        throw new Exception($"Failed to create company. Status: {response.StatusCode}, Error: {responseContent}");
                    }
                }
                else
                {
                    throw new Exception($"Failed to create company. Status: {response.StatusCode}, Error: {responseContent}");
                }
            }

            return response.IsSuccessStatusCode;
        }
    }
}