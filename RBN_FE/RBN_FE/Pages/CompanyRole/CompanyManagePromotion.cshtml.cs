using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManagePromotionModel : PageModel
    {


        private static readonly string APIPort = "http://localhost:5250/api/";
        public List<PromotionFee> PromotionFee { get; set; }
        public List<ViewDetailsPromotionDto> Promotion { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchStatus { get; set; }
        public async Task OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var queryParameters = new List<string>();

                    if (SearchId.HasValue)
                        queryParameters.Add($"id={SearchId.Value}");

                    if (SearchStatus.HasValue)
                        queryParameters.Add($"status={SearchStatus.Value}");

                    var queryString = queryParameters.Any() ? "?" + string.Join("&", queryParameters) : "";

                    var apiUrl = $"{APIPort}Promotion/search{queryString}";

                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        PromotionFee = JsonConvert.DeserializeObject<List<PromotionFee>>(content) ?? new List<PromotionFee>();
                        PromotionFee = PromotionFee.OrderByDescending(f => f.EndTime).ToList();
                    }
                    else
                    {
                        PromotionFee = new List<PromotionFee>();
                    }

                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                    PromotionFee = new List<PromotionFee>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    PromotionFee = new List<PromotionFee>();
                }
            }
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(APIPort + "Promotions"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            // Directly deserialize into a List<ProductDetail>
                            var data = JsonConvert.DeserializeObject<List<ViewDetailsPromotionDto>>(content);

                            if (data != null)
                            {
                                Promotion = data;
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