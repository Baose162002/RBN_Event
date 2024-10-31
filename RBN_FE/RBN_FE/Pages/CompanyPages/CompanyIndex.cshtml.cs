using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace RBN_FE.Pages.CompanyPages
{
    public class CompanyIndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CompanyIndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public List<ListCompanyDTO> Companies { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Companies = await _httpClient.GetFromJsonAsync<List<ListCompanyDTO>>("https://rbnapi20241031155156.azurewebsites.net/api/Company", options);

                if (Companies == null || Companies.Count == 0)
                {
                    Console.WriteLine("Không có công ty nào được trả về từ API.");
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
