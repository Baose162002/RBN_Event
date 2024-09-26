using BusinessObject;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.IRepositories;

namespace Repositories.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDBContext _context;

        public BookingRepository()
        {
            _context ??= new ApplicationDBContext();
        }
        public async Task<List<Booking>> GetAllBooking()
        {
            return await _context.Bookings.Include(u => u.User).Include(e => e.Event).ToListAsync();
        }
        public async Task<Booking> GetBookingById(int id)
        {
            return await _context.Bookings.Include(u => u.User).Include(e => e.Event).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task CreateBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateBooking(Booking booking)
        {
            var tracker = _context.Attach(booking);
            tracker.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteBooking(int id)
        {
            var existedBooking = await _context.Bookings.FindAsync(id);
            if (existedBooking == null)
            {
                throw new Exception("Booking is not existed");
            }
            _context.Remove(existedBooking);
            await _context.SaveChangesAsync();
        }
    }
}
