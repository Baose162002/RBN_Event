using BusinessObject.DTO;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Dto;

namespace Services.IService
{
    public interface IFeedbackService
    {
        Task<List<FeedBack>> GetAllFeedback();
        Task Create(FeedbackDTO feedback);
        Task UpdateFeedback(FeedbackDTO feedback, int id);
        Task Delete(int id);
        Task<FeedBack> GetFeedbackById(int id);
    }
}
