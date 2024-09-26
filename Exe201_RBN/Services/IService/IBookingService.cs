using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IBookingService
    {
        Task<List<ViewDetailsBookingDto>> GetAllBooking();
        Task<ViewDetailsBookingDto> GetBookingById(int id);
        Task CreateBooking(CreateBookingDto createBookingDto);
        Task UpdateBooking(CreateBookingDto updateBookingDto);
        Task DeleteBooking(int id);
    }
}
