using BusinessObject;
using BusinessObject.DTO;
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
        Task<List<Event>> GetAllEvent();
        Task Update(Event updateevent, int id);
    }
}
