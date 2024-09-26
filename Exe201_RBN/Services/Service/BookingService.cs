using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using Repositories.IRepositories;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IBaseRepository<User> _userRepo;
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepo, IBaseRepository<User> userRepo, IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<List<ViewDetailsBookingDto>> GetAllBooking()
        {
            var bookings = await _bookingRepo.GetAllBooking();
            return _mapper.Map<List<ViewDetailsBookingDto>>(bookings);
        }
        public async Task<ViewDetailsBookingDto> GetBookingById(int id)
        {
            var booking = await _bookingRepo.GetBookingById(id);
            return _mapper.Map<ViewDetailsBookingDto>(booking);
        }
        public async Task CreateBooking(CreateBookingDto createBookingDto)
        {
            var user = await _userRepo.GetByIdAsync(createBookingDto.UserId);
            if(user != null)
            {
                var booking = new Booking
                {
                    Email = user.Email,
                    FullName = user.Name,
                    Address = user.Address,
                    Phone = user.Phone,
                    UserId = user.Id,
                    EventId = createBookingDto.EventId,
                    UserNote = createBookingDto.UserNote,
                    BookingDay = DateTime.Now,
                };
                await _bookingRepo.CreateBooking(booking);
            }
            else
            {
                var booking = _mapper.Map<Booking>(createBookingDto);
                await _bookingRepo.CreateBooking(booking);
            }
        }

        public async Task UpdateBooking(CreateBookingDto updateBookingDto)
        {
            var updateBooking = _mapper.Map<Booking>(updateBookingDto);
        }
        public async Task DeleteBooking(int id)
        {
            await _bookingRepo.DeleteBooking(id);
        }
    }
}
