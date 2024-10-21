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
        private readonly IBaseRepository<Company> _companyRepo;
        private readonly IBaseRepository<Event> _eventRepo;
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepo, IBaseRepository<User> userRepo
            , IBaseRepository<Company> companyRepo
            , IBaseRepository<Event> eventRepo
            , IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _companyRepo = companyRepo;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }
        public async Task<List<ViewDetailsBookingDto>> GetBookingsByCompanyIdAsync(int companyId)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
            {
                throw new Exception("Not found company");
            }
            else
            {
                var bookings = await _bookingRepo.GetBookingsByCompanyIdAsync(company.Id);
                return _mapper.Map<List<ViewDetailsBookingDto>>(bookings);
            }
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
            var eventSelected = await _eventRepo.GetByIdAsync(createBookingDto.EventId);
            
                
                var booking = _mapper.Map<Booking>(createBookingDto);
                booking.BookingDay = DateTime.Now; 
                booking.Price = booking.Quantity * (decimal)eventSelected.Price;
                await _bookingRepo.CreateBooking(booking);
          

        }

        public async Task UpdateBooking(CreateBookingDto updateBookingDto)
        {
            var updateBooking = _mapper.Map<Booking>(updateBookingDto);
        }
        public async Task DeleteBooking(int id)
        {
            await _bookingRepo.DeleteBooking(id);
        }
        public async Task<ViewDetailsBookingDto> ChangeStatusBooking(int id)
        {
            var booking = await _bookingRepo.GetBookingById(id);
            if (booking == null)
            {
                throw new KeyNotFoundException();
            }
            if (booking.Status == 0)
            {
                booking.Status = 1;
            }
            else booking.Status = 0;
            await _bookingRepo.UpdateBooking(booking);
            return _mapper.Map<ViewDetailsBookingDto>(booking);
        }
        public async Task<List<ViewDetailsBookingDto>> SearchBookingByCompanyId(int companyId, int? id, string? email, string? fullname, string? address
            , decimal? price, string? phone, int? status, int? eventId)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
            {
                throw new Exception("Not found company");
            }
            else
            {
                var bookings = await _bookingRepo.GetBookingsByCompanyIdAsync(company.Id);
                if (bookings == null)
                {
                    throw new Exception("No data");
                }
                if (id.HasValue)
                {
                    bookings = bookings.Where(x => x.Id == id.Value).ToList();
                }
                if (!string.IsNullOrEmpty(email))
                {
                    bookings = bookings.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(fullname))
                {
                    bookings = bookings.Where(x => x.FullName.ToLower().Contains(fullname.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(address))
                {
                    bookings = bookings.Where(x => x.Address.ToLower().Contains(address.ToLower())).ToList();
                }
                if (price.HasValue)
                {
                    bookings = bookings.Where(x => x.Price == price.Value).ToList();
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    bookings = bookings.Where(x => x.Phone.ToLower().Contains(phone.ToLower())).ToList();
                }
               
                if (status.HasValue)
                {
                    bookings = bookings.Where(x => x.Status == status.Value).ToList();
                }

                var bookingList = bookings.ToList();

                if (bookingList == null)
                {
                    throw new Exception("No data");
                }

                return _mapper.Map<List<ViewDetailsBookingDto>>(bookingList);
            }
        }
    }
}
