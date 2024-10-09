using BusinessObject.Dto;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IResponseService
    {
        Task<List<Response>> GetAllResponse();
        Task<List<ResponseDTO>> GetResponseByFeedbackId(int id);
        Task Create(ResponseDTO response);
        Task UpdateResponse(ResponseDTO response, int id);
        Task Delete(int id);
        Task<Response> GetResponseById(int id);
    }
}
