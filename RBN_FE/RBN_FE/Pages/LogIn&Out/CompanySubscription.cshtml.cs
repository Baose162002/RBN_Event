using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace RBN_FE.Pages.LogIn_Out
{
    public class CompanySubscriptionModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public CompanySubscriptionModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        [BindProperty(SupportsGet = true)]
        public int CompanyId { get; set; }
        public List<ListSubscriptionPackageDTO> SubscriptionPackages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CompanyEmail { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubscriptionPackageId { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostRegisterPackageAsync()
        {
            if (SubscriptionPackageId <= 0)
            {
                return BadRequest("Gói đăng ký không hợp lệ.");
            }
            var companyIdFromSession = HttpContext.Session.GetInt32("CompanyId");
            if (companyIdFromSession == null)
            {
                return RedirectToPage("/LogIn&Out/Login");
            }
            CompanyId = companyIdFromSession.Value;
            var client = _clientFactory.CreateClient();
            var apiUrl = _configuration["ApiSettings:BaseUrl"] + $"/SubscriptionPackage/register-subsciptionpackage?companyId={CompanyId}&subscriptionPackageId={SubscriptionPackageId}";

            var response = await client.PutAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/LogIn&Out/Login");
            }

            ErrorMessage = "Đã xảy ra lỗi khi đăng ký gói. Vui lòng thử lại.";
            return Page();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(CompanyEmail))
            {
                return RedirectToPage("/LogIn&Out/Login");
            }

            var client = _clientFactory.CreateClient();
            var apiUrl = _configuration["ApiSettings:BaseUrl"] + "/SubscriptionPackage";

            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                SubscriptionPackages = JsonConvert.DeserializeObject<List<ListSubscriptionPackageDTO>>(content);
                return Page();
            }

            return RedirectToPage("/Error");
        }
    }
}