﻿using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
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
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        public EventService(IEventRepository eventRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task<EventDTO> ChangeStatusEvent(int id)
        {
            var existedevent = await _eventRepository.GetEventById(id);
            if (existedevent == null)
            {
                throw new KeyNotFoundException();
            }
            if (existedevent.Status == 0)
            {
                existedevent.Status = 1;
            }
            else existedevent.Status = 0;
            await _eventRepository.Update(existedevent, existedevent.Id);
            return _mapper.Map<EventDTO>(existedevent);
        }
        public async Task<List<EventDTO>> GetEventsByCompanyIdAsync(int companyId)
        {
            var company = await _companyRepository.GetCompanyById(companyId);
            if (company == null)
            {
                throw new Exception("Not found company");
            }
            else
            {
                var bookings = await _eventRepository.GetEventsByCompanyIdAsync(company.Id);
                return _mapper.Map<List<EventDTO>>(bookings);
            }
        }
        public async Task Create(CreateEventDto events)
        {
            if (events == null || string.IsNullOrEmpty(events.Title)
               || string.IsNullOrEmpty(events.Name)
               || string.IsNullOrEmpty(events.EventType)
               || string.IsNullOrEmpty(events.Description)
               || events.Quantity == null || events.Price == null || events.CompanyId == null
               || events.CreateAt == null || events.UpdateAt == null)
            {
                throw new ArgumentException("All fieds must be filled");
            }
            if (events.Price < 0)
            {
                throw new ArgumentException("Price must be a positive number");
            }
            if (events.Quantity < 0)
            {
                throw new ArgumentException("Quantity minimum must be a positive number");
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
                Quantity = events.Quantity,
                Description = events.Description,
                Status = 1,
                CompanyId = events.CompanyId,
                CreateBy = events.CreateBy,
                CreateAt = startDate,
                UpdateBy = events.UpdateBy,
                UpdateAt = updateDate,
                EventImgId = events.EventImgId
            };
            await _eventRepository.Create(_mapper.Map<Event>(eventDTO));

        }

        public async Task Delete(int id)
        {
             await _eventRepository.Delete(id);
        }
        public async Task<PagedResult<EventDTO>> GetAllEvent(string? searchTerm, int pageNumber, int pageSize)
        {
            // Call the repository to get the paged result
            var pagedEvents = await _eventRepository.GetAllEvent(searchTerm, pageNumber, pageSize);

            // Map the PagedResult<Event> to PagedResult<EventDTO>
            var mappedResult = new PagedResult<EventDTO>
            {
                Page = pagedEvents.Page,
                PerPage = pagedEvents.PerPage,
                Total = pagedEvents.Total,
                TotalPages = pagedEvents.TotalPages,
                Data = _mapper.Map<List<EventDTO>>(pagedEvents.Data) // Map the Data property
            };

            return mappedResult;
        }

        public async Task<PagedResult<EventDTO>> GetAllEventsByCompanyId(int companyId, string? searchTerm, int pageNumber, int pageSize)
        {
            // Call the repository to get the paged result
            var pagedEvents = await _eventRepository.GetAllEventsByCompanyId(companyId, searchTerm, pageNumber, pageSize);

            // Map the PagedResult<Event> to PagedResult<EventDTO>
            var mappedResult = new PagedResult<EventDTO>
            {
                Page = pagedEvents.Page,
                PerPage = pagedEvents.PerPage,
                Total = pagedEvents.Total,
                TotalPages = pagedEvents.TotalPages,
                Data = _mapper.Map<List<EventDTO>>(pagedEvents.Data) // Map the Data property
            };

            return mappedResult;
        }
        public async Task<PagedResult<EventDTO>> GetEventsByTypeAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            // Call the repository to get the paged result
            var pagedEvents = await _eventRepository.GetAllEvent(searchTerm, pageNumber, pageSize);

            // Map the PagedResult<Event> to PagedResult<EventDTO>
            var mappedResult = new PagedResult<EventDTO>
            {
                Page = pagedEvents.Page,
                PerPage = pagedEvents.PerPage,
                Total = pagedEvents.Total,
                TotalPages = pagedEvents.TotalPages,
                Data = _mapper.Map<List<EventDTO>>(pagedEvents.Data) // Map the Data property
            };

            return mappedResult;
        }
        public async Task<EventDTO> GetEventById(int id)
        {
            var events = await _eventRepository.GetEventById(id);
            EventDTO response = _mapper.Map<EventDTO>(events);

            return response;
        }
        public async Task<List<EventDTO>> SearchEventByCompanyId(int companyId, int? id, DateTime? createDate, string? eventname
           , double? price, int? status)
        {
            var company = await _companyRepository.GetCompanyById(companyId);
            if (company == null)
            {
                throw new Exception("Not found company");
            }
            else
            {
                var events = await _eventRepository.GetEventsByCompanyIdAsync(company.Id);
                if (events == null)
                {
                    throw new Exception("No data");
                }
                if (id.HasValue)
                {
                    events = events.Where(x => x.Id == id.Value).ToList();
                }
                if (createDate.HasValue)
                {
                    events = events.Where(x => x.CreateAt == createDate.Value).ToList();
                }
                if (!string.IsNullOrEmpty(eventname))
                {
                    events = events.Where(x => x.Name.ToLower().Contains(eventname.ToLower())).ToList();
                }
                
                if (price.HasValue)
                {
                    events = events.Where(x => x.Price == price.Value).ToList();
                }
                
                if (status.HasValue)
                {
                    events = events.Where(x => x.Status == status.Value).ToList();
                }

                var eventList = events.ToList();

                if (eventList == null)
                {
                    throw new Exception("No data");
                }

                return _mapper.Map<List<EventDTO>>(eventList);
            }
        }
    public async Task Update(UpdateEventDto events, int id)
        {
            if(events == null || string.IsNullOrEmpty(events.Title)
                || string.IsNullOrEmpty(events.Name)
                || string.IsNullOrEmpty(events.EventType)
                || string.IsNullOrEmpty(events.Description)
                || events.Quantity == null || events.Price == null ||  events.Status == null)
            {
                throw new ArgumentException("Không được để trống");
            }
            var existing = await _eventRepository.GetEventById(id);
            if(existing == null)
            {
                throw new ArgumentException("Event này không tồn tại");
            }
            if (events.Quantity < 0)
            {
                throw new ArgumentException("Số lượng tối thiểu phải là số dương");
            }
            if (events.Price <= 0)
            {
                throw new ArgumentException("Giá không được là số âm ");
            }
            if(events.Status < 0)
            {
                throw new ArgumentException("Status must be a positive number");
            }
            if (events.EventImgId < 0)
            {
                throw new ArgumentException("EventImg must be a positive number");
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
