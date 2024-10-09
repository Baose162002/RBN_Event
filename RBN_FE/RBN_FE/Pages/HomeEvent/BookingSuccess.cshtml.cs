using BusinessObject.Dto.RequestDto;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RBN_FE.Pages.Service;

namespace RBN_FE.Pages.HomeEvent
{
    public class BookingSuccessModel : PageModel
    {
        private readonly PaymentService _vnPayService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BookingSuccessModel(PaymentService vnPayService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _vnPayService = vnPayService;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public VnPaymentResponseModel VnPaymentResult { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var eventIdFromSession = HttpContext.Session.GetString("EventId");

            if (int.TryParse(userIdString, out int userId) && int.TryParse(eventIdFromSession, out int eventId))
            {        

                // Validate the payment and process accordingly
                var paymentResult = _vnPayService.PaymentExecute(Request.Query);

                if (paymentResult.Success)
                {
                    // Retrieve user details from session
                    var fullName = HttpContext.Session.GetString("FullName");
                    var email = HttpContext.Session.GetString("Email");
                    var address = HttpContext.Session.GetString("Address");
                    var phone = HttpContext.Session.GetString("Phone");
                    var userNote = HttpContext.Session.GetString("UserNote");

                    // Create booking object
                    var bookingDto = new CreateBookingDto
                    {
                        EventId = eventId,
                        UserId = userId,
                        FullName = fullName,
                        Email = email,
                        Address = address,
                        Phone = phone,
                        UserNote = userNote
                    };

                    // Save the booking to your database
                    var requestUrl = $"{_configuration["ApiSettings:BaseUrl"]}/Booking";
                    var response = await _httpClient.PostAsJsonAsync(requestUrl, bookingDto);

                    if (response.IsSuccessStatusCode)
                    {
                        // Redirect to confirmation page
                        return RedirectToPage("/HomeEvent/BookingConfirmation");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error saving booking information.");
                    }
                }
                else
                {
                    if (Request.Query.ContainsKey("vnp_ResponseCode") || Request.Query["vnp_ResponseCode"] == "97") // Assuming "97" means canceled
                    {
                        return RedirectToPage("/HomeEvent/BookingCancel"); // Redirect to a cancellation page
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Order ID or User ID.");
            }

            return Page();
        }
    }
}
