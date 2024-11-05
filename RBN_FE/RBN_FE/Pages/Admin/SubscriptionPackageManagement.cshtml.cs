using BusinessObject.Dto.ResponseDto;
using BusinessObject;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RBN_FE.Pages.Admin
{
    public class SubscriptionPackageManagementModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<CompanyWithSubcription> Conpany { get; set; }
        public SubscriptionPackageManagementModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            using(var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(_configuration["ApiSettings:BaseUrl"] + "/Company/subscription"))
                    {
                        if(response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<List<CompanyWithSubcription>>(content);
                            if(result != null)
                            {
                                Conpany = result;
                            }
                        }
                    }
                }catch (Exception ex)
                {
                    Conpany = new List<CompanyWithSubcription>();
                }
            }
        }
    }
}
