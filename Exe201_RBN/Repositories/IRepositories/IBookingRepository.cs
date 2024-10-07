using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IBookingRepository
    {
        Task<List<ViewDetailsBookingDto>> GetBookingsByCompanyIdAsync(int companyId);
        Task<List<Booking>> GetAllBooking();
        Task<Booking> GetBookingById(int id);
        Task CreateBooking(Booking booking);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(int id);
    }
}
