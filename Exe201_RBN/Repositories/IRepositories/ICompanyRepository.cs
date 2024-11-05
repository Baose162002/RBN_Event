using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllCompany();
        Task<Company> GetCompanyById(int id);
        Task Create(Company company);
        Task Delete(int id);
        Task Update(Company updatecompany, int id);
        Task<Company> GetCompanyByIdUser(int id);
        Task<Company> GetCompanyByEmail(string email);
        Task<List<Company>> GetAllCompanyWithSubcription();
    }
}
