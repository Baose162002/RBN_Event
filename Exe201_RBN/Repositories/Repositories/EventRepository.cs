using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Repositories.Data;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class EventRepository : IEventRepositories
    {
        private readonly ApplicationDBContext _context;
        public EventRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Event>> GetAllEvent()
        {
            var events = await _context.Events.Include("EventImg").ToListAsync();
            return events;

        }
    }
}
