﻿using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using MailKit.Search;
using Repositories.IRepositories;
using Repositories.Repositories;
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
        private readonly IUserService _userService;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, IUserService userService)
        {
            _companyRepository = companyRepository;
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<List<ListCompanyDTO>> GetAllCompany()
        {
            var companies = await _companyRepository.GetAllCompany();
            var response = _mapper.Map<List<ListCompanyDTO>>(companies);

            return response;
        }
        public async Task<List<CompanyWithSubcription>> GetAllCompanyWithSubcription()
        {
            var companies = await _companyRepository.GetAllCompanyWithSubcription();
            var response = _mapper.Map<List<CompanyWithSubcription>>(companies);

            return response;
        }
        public async Task Create(CompanyDTO company)
        {
			if (company == null || string.IsNullOrEmpty(company.Name)
				|| string.IsNullOrEmpty(company.Description)
				|| string.IsNullOrEmpty(company.Address)
				|| string.IsNullOrEmpty(company.Phone)
				|| string.IsNullOrEmpty(company.TaxCode))
			{
				throw new ArgumentException("All fieds must be filled");
			}
			Regex phoneRegex = new Regex(@"^0[0-9]{9}$");
			if (!phoneRegex.IsMatch(company.Phone))
			{
				throw new ArgumentException("Phone must be a 10-digit number starting with 0!");
			}
            var existinguser = await _userService.GetUserByIdAsync(company.UserId);
            if(existinguser == null)
            {
                throw new ArgumentException("User is not existed");
            }
			CompanyDTO companyDTO = new CompanyDTO
            {
                Name = company.Name,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Avatar = company.Avatar,
                TaxCode = company.TaxCode,
                UserId = company.UserId
            };
			var updatecompany = _mapper.Map<Company>(companyDTO);
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
        public async Task<ListCompanyDTO> GetCompanyById(int id)
        {
            
            var companies = await _companyRepository.GetCompanyById(id);
            ListCompanyDTO response = _mapper.Map<ListCompanyDTO>(companies);

            return response;
        }

        public async Task<ListCompanyDTO> GetCompanyByIdUser(int id)
        {

            var companies = await _companyRepository.GetCompanyByIdUser(id);
            ListCompanyDTO response = _mapper.Map<ListCompanyDTO>(companies);

            return response;
        }
    }
}
