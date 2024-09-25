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
    public class FeedbackRepository : IFeedbackRepository
    {
        public async Task<List<FeedBack>> GetAllFeedback()
        {
            var _context = new ApplicationDBContext();
            var feedbacks = await _context.FeedBacks.ToListAsync();
            return feedbacks;

        }
        public async Task<FeedBack> GetFeedbackById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.FeedBacks.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task Create(FeedBack Feedback)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetFeedbackById(Feedback.Id);
                if (existing != null)
                {
                    throw new ArgumentException("Feedback already exist");
                }
                var newFeedback = new FeedBack
                {
                    Comment = Feedback.Comment,
                    FeedbackDate = Feedback.FeedbackDate,
                    CompanyId = Feedback.CompanyId,
                    UserId = Feedback.UserId
                };
                await _context.AddAsync(newFeedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Update(FeedBack feedback, int id)
        {
            try
            {
                var _context = new ApplicationDBContext();
                var existing = await GetFeedbackById(id);
                if (existing != null)
                {
                    existing.Comment = feedback.Comment;
                    existing.FeedbackDate = feedback.FeedbackDate;
                    existing.CompanyId = feedback.CompanyId;
                    existing.UserId = feedback.UserId;
                }
                _context.FeedBacks.Update(existing);
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
                var existing = await GetFeedbackById(id);
                if (existing == null)
                {
                    throw new Exception("Feedback not found");
                }
                _context.FeedBacks.Remove(existing);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
