using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RBN_FE.Pages.HomeEvent
{
    public class HomeEventDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EventDTO EventDetail { get; set; } // Property to hold event details
        public List<EventDTO> RelatedEvents { get; set; } // Property to hold related events
        [BindProperty]
        public CreateBookingDto Booking { get; set; } // Property to hold booking information

        public HomeEventDetailModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task OnGetAsync(int id) // Expecting id as a parameter
        {
            // Fetch event details
            var requestUrl = $"{_configuration["ApiSettings:BaseUrl"]}/Event/{id}";
            EventDetail = await _httpClient.GetFromJsonAsync<EventDTO>(requestUrl);

            // Fetch related events based on EventType
            if (EventDetail != null)
            {
                var searchTerm = EventDetail.EventType;
                var relatedRequestUrl = $"{_configuration["ApiSettings:BaseUrl"]}/Event/eventype?searchTerm={Uri.EscapeDataString(searchTerm)}&pageNumber=1&pageSize=10";

                // Fetch related events from the API
                var relatedResult = await _httpClient.GetFromJsonAsync<PagedResult<EventDTO>>(relatedRequestUrl);

                if (relatedResult != null)
                {
                    RelatedEvents = relatedResult.Data?.Where(e => e.Id != id).ToList() ?? new List<EventDTO>();
                }
                else
                {
                    RelatedEvents = new List<EventDTO>();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the user is logged in by checking the session
            var userId = HttpContext.Session.GetInt32("UserId"); // Change this based on how you store UserId

            if (userId == null)
            {
                // Redirect to the login page if not logged in
                return RedirectToPage("/Login&Out/Login"); // Adjust the redirect path as needed
            }

            // Set the UserId and EventId for the booking
            Booking.UserId = userId.Value; // Assuming UserId is stored as an int
            Booking.EventId = EventDetail.Id; // Ensure you have the EventId set

            // Make a request to your booking API
            var response = await _httpClient.PostAsJsonAsync($"{_configuration["ApiSettings:BaseUrl"]}/Booking", Booking);

            if (response.IsSuccessStatusCode)
            {
                // Optionally, you can show a success message or redirect
                return RedirectToPage("/BookingSuccess"); // Redirect after successful booking
            }
            else
            {
                // Handle the error response (optional)
                ModelState.AddModelError("", "Error occurred while booking. Please try again.");
            }

            // If we reach here, something went wrong; return to the same page
            return Page();
        }
    }

}
