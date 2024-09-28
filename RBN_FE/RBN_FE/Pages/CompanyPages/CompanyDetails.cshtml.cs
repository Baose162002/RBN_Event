using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RBN_FE.Pages.CompanyPages
{
    public class CompanyDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CompanyDetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public ListCompanyDTO Company { get; set; }

        public async Task OnGetAsync(int companyId)
        {
            try
            {

                Company = await _httpClient.GetFromJsonAsync<ListCompanyDTO>($"http://localhost:5250/api/Company/companyid?companyid={companyId}");

                if (Company == null)
                {
                    Console.WriteLine("Không có thông tin công ty.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Lỗi khi gọi API: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Lỗi khi parse JSON: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi không xác định: {e.Message}");
            }
        }
    }
}
