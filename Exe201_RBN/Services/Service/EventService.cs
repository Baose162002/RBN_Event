using Repositories.Data;
using Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class EventService:IEventService
    {
        private readonly IEventRepositories _eventRepositories;
        public EventService(IEventRepositories eventRepositories)
        {
            _eventRepositories = eventRepositories;
        }
        public async Task<List<Event>> GetAllEvent()
        {
            return await _eventRepositories.GetAllEvent();
        }

    }
}
