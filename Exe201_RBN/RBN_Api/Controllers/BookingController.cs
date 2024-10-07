using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ViewDetailsBookingDto>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBooking();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ViewDetailsBookingDto>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            await _bookingService.CreateBooking(createBookingDto);
            return Ok(new { message = "Tạo đơn thành công" });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBooking([FromBody] CreateBookingDto updateBookingDto)
        {
            if (updateBookingDto == null)
            {
                return BadRequest("Thông tin đơn đặt không hợp lệ.");
            }

            await _bookingService.UpdateBooking(updateBookingDto);
            return Ok(new { message = "Cập nhật đơn thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {

            await _bookingService.DeleteBooking(id);
            return Ok(new { message = "Xóa đơn thành công" });
        }
        [HttpPut("change-status-booking/{id}")]
        public async Task<ViewDetailsBookingDto> ChangeStatusBooking(int id)
        {
            var booking = await _bookingService.ChangeStatusBooking(id);
            return booking;
        }
        [HttpGet("search")]
        public async Task<List<ViewDetailsBookingDto>> SearchBooking(int? id, string? email, string? fullname, string? address
            , decimal? price, string? phone, DateTime? bookingDay, int? status, int? eventId)
        {
            return await _bookingService.SearchBooking(id, email, fullname, address, price, phone, bookingDay, status, eventId);
        }
        [HttpGet("get-booking-by-company/{id}")]
        public async Task<List<ViewDetailsBookingDto>> GetBookingsByCompanyIdAsync(int id)
        {
            return await _bookingService.GetBookingsByCompanyIdAsync(id);
        }
    }
}
