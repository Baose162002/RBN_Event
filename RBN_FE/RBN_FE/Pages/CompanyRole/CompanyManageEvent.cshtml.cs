using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManageEventModel : PageModel
    {
        public List<EventDTO> Event { get; set; }
        private static string APIPort = "http://localhost:5250/api/";
        public async Task OnGetAsync(int id)
        {
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        using (var response = await httpClient.GetAsync(APIPort + "Event/get-event-by-company/" + 1))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<List<EventDTO>>(content);

                                if (result != null)
                                {
                                    Event = result;
                                }
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Request error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error: {ex.Message}");
                    }
                }
            }
        }
    }
}