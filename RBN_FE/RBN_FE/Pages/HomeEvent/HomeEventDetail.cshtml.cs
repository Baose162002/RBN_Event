using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RBN_FE.Pages.Service;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RBN_FE.Pages.HomeEvent
{
    public class HomeEventDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly VnPayService _vnPayService;

        public EventDTO EventDetail { get; set; } // Property to hold event details
        public List<EventDTO> RelatedEvents { get; set; } // Property to hold related events
        [BindProperty]
        public CreateBookingDto Booking { get; set; } // Property to hold booking information



        public HomeEventDetailModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, VnPayService vnPayService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _vnPayService = vnPayService;
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Login&Out/Login");
            }

            if (int.TryParse(userIdString, out int userId))
            {
                var requestUrl = $"{_configuration["ApiSettings:BaseUrl"]}/Event/{id}";
                EventDetail = await _httpClient.GetFromJsonAsync<EventDTO>(requestUrl);

                if (EventDetail != null)
                {
                    Booking.UserId = userId;
                    Booking.EventId = EventDetail.Id;

                    // Lấy giá từ EventDetail
                    var eventPrice = EventDetail.Price; // Đảm bảo EventDetail đã được nạp

                    if (eventPrice <= 0)
                    {
                        ModelState.AddModelError("", "Giá sự kiện không hợp lệ.");
                        return Page();
                    }

                    // Tạo URL thanh toán
                    var paymentUrl = _vnPayService.GeneratePaymentUrl(eventPrice, Booking);
                    return Redirect(paymentUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Event details not found. Please try again.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserId. Please try again.");
            }

            return Page();
        }
    }




}
