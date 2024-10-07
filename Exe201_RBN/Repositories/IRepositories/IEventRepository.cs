using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IEventRepository
    {
        Task Delete(int id);
        Task Create(Event events);
        Task<Event> GetEventById(int id);
        Task<PagedResult<Event>> GetAllEvent(string? searchTerm, int pageNumber, int pageSize);
        Task Update(Event updateevent, int id);
        Task<PagedResult<Event>> GetAllEventsByCompanyId(int companyId, string? searchTerm, int pageNumber, int pageSize);
        Task<PagedResult<Event>> GetEventsByTypeAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
