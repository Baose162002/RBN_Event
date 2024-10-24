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

        public List<ListSubscriptionPackageDTO> SubscriptionPackages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CompanyEmail { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(CompanyEmail))
            {
                return RedirectToPage("/LogIn_Out/Login");
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