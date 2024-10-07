using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IEventService
    {
        Task Delete(int id);
        Task Update(UpdateEventDto events, int id);
        Task Create(CreateEventDto events);
        Task<EventDTO> GetEventById(int id);
        Task<PagedResult<EventDTO>> GetAllEvent(string? searchTerm, int pageNumber, int pageSize);
        Task<List<EventDTO>> GetEventsByCompanyIdAsync(int companyId);
        Task<PagedResult<EventDTO>> GetAllEventsByCompanyId(int companyId, string? searchTerm, int pageNumber, int pageSize);
        Task<PagedResult<EventDTO>> GetEventsByTypeAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
