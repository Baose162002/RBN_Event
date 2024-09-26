﻿using BusinessObject;
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
    }
}