using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RBN_FE.Pages.Shared
{
    public class _HeaderModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public _HeaderModel(IHttpClientFactory httpClientFactory)
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

                Companies = await _httpClient.GetFromJsonAsync<List<ListCompanyDTO>>("http://localhost:5250/api/Company", options);

                if (Companies == null || Companies.Count == 0)
                {
                    Console.WriteLine("Không có công ty nào ???c tr? v? t? API.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"L?i khi g?i API: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"L?i khi parse JSON: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"L?i không xác ??nh: {e.Message}");
            }
        }
    }
}
