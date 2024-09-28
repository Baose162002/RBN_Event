using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICompanyService
    {
        Task<List<ListCompanyDTO>> GetAllCompany();
        Task Create(CompanyDTO company);
        Task UpdateCompany(CompanyDTO company, int id);
        Task Delete(int id);
        Task<ListCompanyDTO> GetCompanyById(int id);
    }
}
