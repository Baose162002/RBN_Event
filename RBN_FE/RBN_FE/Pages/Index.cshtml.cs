using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RBN_FE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public List<EventDTO> Events { get; set; }
        public string SearchTerm { get; set; } = string.Empty; // Property to hold searchTerm
        public int PageNumber { get; set; } = 1; // Property for pageNumber
        public int PageSize { get; set; } = 10; // Property for pageSize
        public int TotalPages { get; set; } // Property for total pages

        public async Task OnGetAsync(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            SearchTerm = searchTerm; // Assign value to property
            PageNumber = pageNumber; // Assign value to property
            PageSize = pageSize; // Assign value to property

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Create URL with search and pagination parameters
                var searchTermValue = searchTerm ?? string.Empty; // If searchTerm is null, use an empty string

                var requestUrl = $"{_configuration["ApiSettings:BaseUrl"]}/Event?searchTerm={Uri.EscapeDataString(searchTermValue)}&pageNumber={pageNumber}&pageSize={pageSize}";

                // Fetch events from the API
                var result = await _httpClient.GetFromJsonAsync<PagedResult<EventDTO>>(requestUrl, options);

                // Assign results
                if (result != null)
                {
                    Events = result.Data; // Assign event data
                    TotalPages = result.TotalPages; // Assign total pages
                }
                else
                {
                    Events = new List<EventDTO>(); // If no result, initialize to empty
                    TotalPages = 0; // Set total pages to 0
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error calling API: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Error parsing JSON: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e.Message}");
            }
        }
    }
}
