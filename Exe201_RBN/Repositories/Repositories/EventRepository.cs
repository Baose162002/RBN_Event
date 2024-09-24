using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class EventRepository : IEventRepository
    {
        public async Task<List<Event>> GetAllEvent()
        {
            var _context = new ApplicationDBContext();
            var events = await _context.Events.ToListAsync();
            return events;
        }

        public async Task<Event> GetEventById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(Event events)
        {
            var _context = new ApplicationDBContext();
            _context.Events.Add(events);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Event events, int id)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetEventById(id);
                if (existing == null)
                {
                    throw new ArgumentException("Event not found");
                }
                existing.Title = events.Title;
                existing.Name = events.Name;
                existing.EventType = events.EventType;
                existing.Price = events.Price;
                existing.MinCapacity = events.MinCapacity;
                existing.MaxCapacity = events.MaxCapacity;
                existing.Description = events.Description;
                existing.Status = events.Status;
                existing.CompanyId = events.CompanyId;
                existing.CreateBy = events.CreateBy;
                existing.CreateAt = events.CreateAt;
                existing.UpdateBy = events.UpdateBy;
                existing.UpdateAt = events.UpdateAt;
                _context.Events.Update(existing);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                throw e;
            }

            
        }

        public async Task Delete(int id)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetEventById(id);
                if (existing == null)
                {
                    throw new ArgumentException("Event not found");
                }
                _context.Events.Remove(existing);
                await _context.SaveChangesAsync();

            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
