using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task Create(EventDTO events)
        {
            if (events == null || string.IsNullOrEmpty(events.Title)
              || string.IsNullOrEmpty(events.Name)
              || string.IsNullOrEmpty(events.EventType)
              || string.IsNullOrEmpty(events.Description)
              || string.IsNullOrEmpty(events.CreateBy)
              || string.IsNullOrEmpty(events.UpdateBy))
            {
                throw new ArgumentException("All fieds must be filled");
            }
            if (events.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be a positive number");
            }

            string[] dateFormats = { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy" };
            DateTime startDate, updateDate;
            if (!DateTime.TryParseExact(events.CreateAt, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                throw new ArgumentException("Invalid create date format", nameof(events.CreateAt));
            }
            if (!DateTime.TryParseExact(events.UpdateAt, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out updateDate))
            {
                throw new ArgumentException("Invalid update date format", nameof(events.UpdateAt));
            }
            Event eventDTO = new Event
            {
                Title = events.Title,
                Name = events.Name,
                EventType = events.EventType,
                Price = events.Price,
                Capacity = events.Capacity,
                Description = events.Description,
                Status = events.Status,
                CompanyId = events.CompanyId,
                CreateBy = events.CreateBy,
                CreateAt = startDate,
                UpdateBy = events.UpdateBy,
                UpdateAt = updateDate
            };
            await _eventRepository.Create(_mapper.Map<Event>(eventDTO));

        }

        public async Task Delete(int id)
        {
             await _eventRepository.Delete(id);
        }

        public async Task<List<EventDTO>> GetAllEvent()
        {
            var events = await _eventRepository.GetAllEvent();

           
            var eventDTOs = _mapper.Map<List<EventDTO>>(events);

            return eventDTOs;

        }

        public async Task<EventDTO> GetEventById(int id)
        {
            var events = await _eventRepository.GetEventById(id);
            EventDTO response = _mapper.Map<EventDTO>(events);

            return response;
        }

        public async Task Update(EventDTO events, int id)
        {
            if(events == null || string.IsNullOrEmpty(events.Title)
                || string.IsNullOrEmpty(events.Name)
                || string.IsNullOrEmpty(events.EventType)
                || string.IsNullOrEmpty(events.Description)
                || string.IsNullOrEmpty(events.CreateBy)
                || string.IsNullOrEmpty(events.UpdateBy)
                || events.Capacity == null || events.Price == null ||  events.Status == null || events.CreateBy == null || events.UpdateBy == null || events.CompanyId == null
                || events.CreateAt == null || events.UpdateAt == null)
            {
                throw new ArgumentException("All fieds must be filled");
            }
            if (events.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be a positive number");
            }
            if (events.Price <= 0)
            {
                throw new ArgumentException("Price must be a positive number");
            }

            string[] dateFormats = { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy" };
            DateTime startDate, updateDate;
            if (!DateTime.TryParseExact(events.CreateAt, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                throw new ArgumentException("Invalid create date format", nameof(events.CreateAt));
            }
            if (!DateTime.TryParseExact(events.UpdateAt, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out updateDate))
            {
                throw new ArgumentException("Invalid update date format", nameof(events.UpdateAt));
            }

        
            var updateevent = _mapper.Map<Event>(events);
            await _eventRepository.Update(updateevent, id);
        }
    }
}
