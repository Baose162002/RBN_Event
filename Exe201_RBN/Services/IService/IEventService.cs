using BusinessObject;
using BusinessObject.DTO;
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
        Task Update(EventDTO events, int id);
        Task Create(EventDTO events);
        Task<EventDTO> GetEventById(int id);
        Task<List<EventDTO>> GetAllEvent();
    }
}
