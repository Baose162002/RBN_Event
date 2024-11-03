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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        [Required(ErrorMessage = "Avatar is required")]
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
                string avatarUrl = null;
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    avatarUrl = await UploadAvatar();
                }

                var companyCreated = await CreateCompany(avatarUrl);
                HttpContext.Session.SetString("CompanyEmail", CreateCompanyDto.Email);
                TempData["CompanyEmail"] = CreateCompanyDto.Email;
                if (companyCreated)
                {
                    return new JsonResult(new
                    {
                        success = true,
                        message = "Đăng ký công ty thành công! Vui lòng chọn gói đăng ký.",
                        redirectUrl = $"/LogIn_Out/CompanySubscription?companyEmail={CreateCompanyDto.Email}"
                    });
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

            var response = await client.PostAsync(apiUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    try
                    {
                        var errorMessage = JObject.Parse(responseContent)["message"]?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            // Hiển thị thông báo lỗi cụ thể từ API
                            throw new Exception(errorMessage);
                        }
                        else
                        {
                            throw new Exception("Xác thực không thành công. Vui lòng kiểm tra lại các trường đầu vào.");
                        }
                    }
                    catch (JsonException)
                    {
                        throw new Exception("Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.");
                    }
                }
                else
                {
                    throw new Exception("Đã xảy ra lỗi. Vui lòng thử lại.");
                }
            }

            return response.IsSuccessStatusCode;
        }
    }
}