using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManageBookingModel : PageModel
    {
        // List to hold the booking data
        public List<ViewDetailsBookingDto> Booking { get; set; }

        // Base API URL
        private static readonly string APIPort = "http://localhost:5250/api/";

        // Bind properties to capture search inputs from query string
        [BindProperty(SupportsGet = true)]
        public int? SearchId { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? SearchStatus { get; set; }

        // Handle GET requests
        public async Task OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Build the query string based on provided search parameters
                    var queryParameters = new List<string>();

                    if (SearchId.HasValue)
                        queryParameters.Add($"id={SearchId.Value}");

                  

                    if (SearchStatus.HasValue)
                        queryParameters.Add($"status={SearchStatus.Value}");

                    // Combine query parameters
                    var queryString = queryParameters.Any() ? "?" + string.Join("&", queryParameters) : "";

                    // Construct the full API URL
                    var apiUrl = $"{APIPort}Booking/search{queryString}";

                    // Make the GET request to the API
                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Booking = JsonConvert.DeserializeObject<List<ViewDetailsBookingDto>>(content) ?? new List<ViewDetailsBookingDto>();
                    }
                    else
                    {
                        // Handle unsuccessful response
                        Booking = new List<ViewDetailsBookingDto>();
                        // Optionally, log or handle specific status codes
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle request exceptions
                    Console.WriteLine($"Request error: {ex.Message}");
                    Booking = new List<ViewDetailsBookingDto>();
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Booking = new List<ViewDetailsBookingDto>();
                }
            }
        }
    }
}
