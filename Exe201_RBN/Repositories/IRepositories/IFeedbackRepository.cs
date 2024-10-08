using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IFeedbackRepository
    {
        Task<List<FeedBack>> GetFeedBacksByCompanyIdAsync(int companyId);
        Task<List<FeedBack>> GetAllFeedback();
        Task<FeedBack> GetFeedbackById(int id);
        Task Create(FeedBack feedback);
        Task Delete(int id);
        Task Update(FeedBack updatefeedback, int id);
    }
}
