using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RBN_FE.Pages.LogIn_Out
{
    public class SignupModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public SignupModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Yêu cầu nhập tên của bạn")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Yêu cầu nhập email của bạn")]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Yêu cầu nhập số điện thoại của bạn")]
            [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Yêu cầu nhập địa chỉ của bạn")]
            public string Address { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) }) { StatusCode = 400 };
            }

            try
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];

                // Kiểm tra email và số điện thoại tồn tại
                var checkEmailResponse = await client.GetAsync($"{apiBaseUrl}/User/check-email?email={Input.Email}");
                var checkPhoneResponse = await client.GetAsync($"{apiBaseUrl}/User/check-phone?phone={Input.Phone}");

                if (checkEmailResponse.IsSuccessStatusCode && bool.Parse(await checkEmailResponse.Content.ReadAsStringAsync()))
                {
                    return new JsonResult(new { success = false, message = "Email đã tồn tại." }) { StatusCode = 400 };
                }

                if (checkPhoneResponse.IsSuccessStatusCode && bool.Parse(await checkPhoneResponse.Content.ReadAsStringAsync()))
                {
                    return new JsonResult(new { success = false, message = "Số điện thoại đã tồn tại." }) { StatusCode = 400 };
                }

                // Tạo người dùng nếu email và số điện thoại chưa tồn tại
                var response = await client.PostAsJsonAsync($"{apiBaseUrl}/User/create-user", Input);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent);
                    return new JsonResult(new { success = true, message = apiResponse.message });
                }
                else
                {
                    return new JsonResult(new { success = false, message = responseContent }) { StatusCode = (int)response.StatusCode };
                }
            }
            catch (Exception ex)
            {
                // Trả về lỗi tổng quát cho người dùng
                return new JsonResult(new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu" }) { StatusCode = 500 };
            }
        }


        public class ApiResponse
        {
            public string message { get; set; }
        }
    }
}
