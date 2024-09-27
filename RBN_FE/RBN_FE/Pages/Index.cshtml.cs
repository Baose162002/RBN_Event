using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RBN_FE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public List<EventDTO> Events { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Events = await _httpClient.GetFromJsonAsync<List<EventDTO>>("http://localhost:5250/api/Event", options);

                if (Events == null || Events.Count == 0)
                {
                    Console.WriteLine("Không có sự kiện nào được trả về từ API.");
                }
                else
                {
                    Console.WriteLine($"Số lượng sự kiện nhận được: {Events.Count}");
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
