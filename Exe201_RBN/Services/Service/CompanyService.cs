using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CompanyService:ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task<List<Company>> GetAllCompany()
        {
            return await _companyRepository.GetAllCompany();
        }
        public async Task Create(CompanyDTO company)
        {
            var updatecompany = _mapper.Map<Company>(company);
            await _companyRepository.Create(updatecompany);
        }
        public async Task UpdateCompany(CompanyDTO company, int id)
        {
            if (company == null || string.IsNullOrEmpty(company.Name)
                || string.IsNullOrEmpty(company.Description)
                || string.IsNullOrEmpty(company.Address)
                || string.IsNullOrEmpty(company.Phone)
                || string.IsNullOrEmpty(company.TaxCode))
            {
                throw new ArgumentException("All fieds must be filled");
            }
            Company existing;
            string newCompany = null;
            existing = await _companyRepository.GetCompanyById(id);
            if (existing == null)
            {
                throw new ArgumentException("Company is not existed");
            }
            Regex phoneRegex = new Regex(@"^0[0-9]{9}$");
            if (!phoneRegex.IsMatch(company.Phone))
            {
                throw new ArgumentException("Phone must be a 10-digit number starting with 0!");
            }


            var updatecompany = _mapper.Map<Company>(company);
            await _companyRepository.Update(updatecompany, id);
            
        }
        public async Task Delete(int id)
        {
            await _companyRepository.Delete(id);
        }
        public async Task<Company> GetCompanyById(int id)
        {
            return await _companyRepository.GetCompanyById(id);
        }


    }
}
