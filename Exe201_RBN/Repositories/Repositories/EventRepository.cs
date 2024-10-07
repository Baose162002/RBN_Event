using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class EventRepository : IEventRepository
    {
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Chuẩn hóa chuỗi và loại bỏ dấu
            text = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in text)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public async Task<List<Event>> GetEventsByCompanyIdAsync(int companyId)
        {
            using var _context = new ApplicationDBContext();
            var events = await _context.Events
                .Include(b => b.EventImg)
                .Include(e => e.Company)
                .Where(b => b.Company.Id == companyId)
                .ToListAsync();

            return events;
        }
        public async Task<PagedResult<Event>> GetAllEvent(string? searchTerm, int pageNumber, int pageSize)
        {
            using var _context = new ApplicationDBContext(); // Use a using statement to ensure proper disposal

            // Start with a basic query
            var query = _context.Events
                .Include(e => e.Company)   // Include related Company
                .Include(e => e.EventImg)
                .Where(e => e.Status == 1) // Only get events with status = 1
                .AsQueryable();

            // Get total count before applying pagination
            var totalCount = await query.CountAsync();

            // If searchTerm is provided, retrieve all events and filter in memory
            List<Event> events = new List<Event>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Retrieve all events and filter in memory
                events = await query.ToListAsync(); // Get all matching events
                var normalizedSearchTerm = RemoveDiacritics(searchTerm.ToLower());

                // Filter by searchTerm after fetching data
                events = events.Where(e =>
                    RemoveDiacritics(e.Title.ToLower()).Contains(normalizedSearchTerm) ||
                    RemoveDiacritics(e.Name.ToLower()).Contains(normalizedSearchTerm) ||
                    RemoveDiacritics(e.EventType.ToLower()).Contains(normalizedSearchTerm)).ToList();
            }
            else
            {
                // If no search term is provided, get events directly
                events = await query.ToListAsync();
            }

            // Apply pagination
            var pagedEvents = events
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Create and return PagedResult
            return new PagedResult<Event>
            {
                Page = pageNumber,
                PerPage = pageSize,
                Total = totalCount,
                TotalPages = totalPages,
                Data = pagedEvents
            };
        }

        public async Task<PagedResult<Event>> GetAllEventsByCompanyId(int companyId, string? searchTerm, int pageNumber, int pageSize)
        {
            using var _context = new ApplicationDBContext(); // Use a using statement to ensure proper disposal

            // Start with a query that filters by companyId and event status
            var query = _context.Events
                .Include(e => e.Company)   // Include related Company
                .Include(e => e.EventImg)
                .Where(e => e.CompanyId == companyId && e.Status == 1) // Filter by companyId and status = 1
                .AsQueryable();

            // Get total count before applying pagination
            var totalCount = await query.CountAsync();

            // List to hold filtered events
            List<Event> events = new List<Event>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Retrieve all events that match the companyId and filter by search term in memory
                events = await query.ToListAsync(); // Fetch all events matching the companyId
                var normalizedSearchTerm = RemoveDiacritics(searchTerm.ToLower());

                // Filter by searchTerm after fetching data
                events = events.Where(e =>
                    RemoveDiacritics(e.Title.ToLower()).Contains(normalizedSearchTerm) ||
                    RemoveDiacritics(e.Name.ToLower()).Contains(normalizedSearchTerm) ||
                    RemoveDiacritics(e.EventType.ToLower()).Contains(normalizedSearchTerm)).ToList();
            }
            else
            {
                // If no search term is provided, get events directly
                events = await query.ToListAsync();
            }

            // Apply pagination
            var pagedEvents = events
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Create and return PagedResult
            return new PagedResult<Event>
            {
                Page = pageNumber,
                PerPage = pageSize,
                Total = totalCount,
                TotalPages = totalPages,
                Data = pagedEvents
            };
        }
        public async Task<PagedResult<Event>> GetEventsByTypeAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            using var _context = new ApplicationDBContext(); // Use a using statement to ensure proper disposal

            // Start with a basic query
            var query = _context.Events
                .Include(e => e.Company)   // Include related Company
                .Include(e => e.EventImg)
                .Where(e => e.Status == 1) // Only get events with status = 1
                .AsQueryable();

            // Get total count before applying pagination
            var totalCount = await query.CountAsync();

            // If searchTerm is provided, retrieve all events and filter in memory
            List<Event> events = new List<Event>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Retrieve all events and filter in memory
                events = await query.ToListAsync(); // Get all matching events
                var normalizedSearchTerm = RemoveDiacritics(searchTerm.ToLower());

                // Filter by searchTerm after fetching data
                events = events.Where(e =>
                    RemoveDiacritics(e.EventType.ToLower()).Contains(normalizedSearchTerm)).ToList();
            }
            else
            {
                // If no search term is provided, get events directly
                events = await query.ToListAsync();
            }

            // Apply pagination
            var pagedEvents = events
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Create and return PagedResult
            return new PagedResult<Event>
            {
                Page = pageNumber,
                PerPage = pageSize,
                Total = totalCount,
                TotalPages = totalPages,
                Data = pagedEvents
            };
        }

        public async Task<Event> GetEventById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.Events.Include(e => e.Company).Include(e => e.EventImg).FirstOrDefaultAsync(e => e.Id == id);
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
                if (existing != null)
                {
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
                    var eventImg = await _context.EventImgs.FindAsync(events.EventImgId);
                    if (eventImg == null)
                    {
                        throw new ArgumentException("Invalid EventImgId");
                    }

                    existing.EventImgId = events.EventImgId;
                    _context.Entry(existing).Property(e => e.EventImgId).IsModified = true;


                }
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

                // Thay vì xóa, ta chỉ cập nhật status thành giá trị đã xóa (ví dụ: 0)
                existing.Status = 2;  // 0 hoặc bất kỳ giá trị nào bạn quy ước để đánh dấu sự kiện đã bị xóa

                _context.Events.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
