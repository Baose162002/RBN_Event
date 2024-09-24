using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllCompany();
        Task Create(CompanyDTO company);
        Task UpdateCompany(CompanyDTO company, int id);
        Task Delete(int id);
        Task<Company> GetCompanyById(int id);
    }
}
