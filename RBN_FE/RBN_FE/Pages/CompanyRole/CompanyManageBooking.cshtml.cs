using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManageBookingModel : PageModel
    {
        // List to hold the bookings data
        public List<ViewDetailsBookingDto> Booking { get; set; }

        public ViewDetailsBookingDto BookingDetails { get; set; }

        private static readonly string APIPort = "http://localhost:5250/api/";
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 8;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? BookingId { get; set; }

        public async Task<IActionResult> OnPostChangeStatusAsync()
        {
            if (BookingId == null)
            {
                ModelState.AddModelError("", "BookingId không hợp lệ.");
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gọi API để thay đổi trạng thái booking
                    var apiUrl = $"{APIPort}Booking/change-status-booking/{BookingId.Value}";
                    using (var response = await httpClient.PutAsync(apiUrl, null)) 
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Trạng thái booking đã được cập nhật thành công.";
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không thể thay đổi trạng thái booking.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi gửi yêu cầu: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi không mong đợi: {ex.Message}");
                }
            }

            await OnGetAsync();

            return RedirectToPage("/CompanyRole/CompanyManageBooking");
        }
        public async Task OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    if (BookingId.HasValue)
                    {
                        var bookingApiUrl = $"{APIPort}Booking/{BookingId.Value}";
                        var bookingResponse = await httpClient.GetAsync(bookingApiUrl);

                        if (bookingResponse.IsSuccessStatusCode)
                        {
                            var bookingContent = await bookingResponse.Content.ReadAsStringAsync();
                            BookingDetails = JsonConvert.DeserializeObject<ViewDetailsBookingDto>(bookingContent);
                        }
                        else
                        {
                            BookingDetails = null;
                        }
                    }
                    else
                    {
                        var queryParameters = new List<string>();

                        if (SearchId.HasValue)
                            queryParameters.Add($"id={SearchId.Value}");

                        if (SearchStatus.HasValue)
                            queryParameters.Add($"status={SearchStatus.Value}");

                        var queryString = queryParameters.Any() ? "?" + string.Join("&", queryParameters) : "";

                        var apiUrl = $"{APIPort}Booking/search{queryString}";

                        var response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            Booking = JsonConvert.DeserializeObject<List<ViewDetailsBookingDto>>(content) ?? new List<ViewDetailsBookingDto>();
                            TotalPages = (int)Math.Ceiling((double)Booking.Count() / PageSize);
                            Booking = Booking.OrderByDescending(f => f.BookingDay)
                        .Skip((PageIndex - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                        }
                        else
                        {
                            Booking = new List<ViewDetailsBookingDto>();
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                    Booking = new List<ViewDetailsBookingDto>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Booking = new List<ViewDetailsBookingDto>();
                }
            }
        }
    }
}
