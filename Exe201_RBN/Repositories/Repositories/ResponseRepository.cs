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
    public class ResponseRepository : IResponseRepository
    {
        public async Task<List<Response>> GetResponseByFeedbackId(int id)
        {
            var _context = new ApplicationDBContext();
            var response = await _context.Responses
                .Include(x=> x.FeedBack)
                .Include(x=>x.Company)
                .Where(x=>x.FeedBack.Id == id).ToListAsync();
            return response;
        }
        public async Task<List<Response>> GetAllResponse()
        {
            var _context = new ApplicationDBContext();
            var responses = await _context.Responses.ToListAsync();
            return responses;

        }
        public async Task<Response> GetResponseById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.Responses.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task Create(Response Repsonse)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetResponseById(Repsonse.Id);
                if (existing != null)
                {
                    throw new ArgumentException("Response already exist");
                }
                var newResponse = new Response
                {
                    Comment = Repsonse.Comment,
                    ResponseDate = Repsonse.ResponseDate,
                    FeedBackId = Repsonse.FeedBackId,
                    CompanyId = Repsonse.CompanyId
                };
                await _context.AddAsync(newResponse);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Update(Response response, int id)
        {
            try
            {
                var _context = new ApplicationDBContext();
                var existing = await GetResponseById(id);
                if (existing != null)
                {
                    existing.Comment = response.Comment;
                    existing.ResponseDate = response.ResponseDate;
                    existing.FeedBackId = response.CompanyId;
                    existing.CompanyId = response.CompanyId;
                }
                _context.Responses.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task Delete(int id)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetResponseById(id);
                if (existing == null)
                {
                    throw new Exception("Reponse not found");
                }
                _context.Responses.Remove(existing);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
