﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IResponseRepository
    {
        Task<List<Response>> GetResponsesByCompanyIdAsync(int companyId);
        Task<List<Response>> GetResponseByFeedbackId(int id);
        Task<List<Response>> GetAllResponse();
        Task<Response> GetResponseById(int id);
        Task Create(Response response);
        Task Delete(int id);
        Task Update(Response updateresponses, int id);
    }
}
