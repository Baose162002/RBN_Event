using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Service
{
    public class SubscriptionPackageService : ISubscriptionPackageService
    {
        private readonly ISubscriptionPackageRepository _subscriptionPackageRepository;
        private readonly IBaseRepository<Company> _companyRepo;
        private readonly IMapper _mapper;

        public SubscriptionPackageService(ISubscriptionPackageRepository subscriptionPackageRepository, IBaseRepository<Company> companyRepo, IMapper mapper)
        {
            _subscriptionPackageRepository = subscriptionPackageRepository;
            _companyRepo = companyRepo;
            _mapper = mapper;
        }
        public async Task CompanyRegisterSubscriptionPackage(int companyId, int subscriptionId)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
            {
                throw new Exception("Not found company");
            }
            var subscriptionPackage = await GetSubscriptionPackageById(subscriptionId);
            if (subscriptionPackage == null)
            {
                throw new Exception("Subscription package not found");
            }
            company.SubscriptionPackageId = subscriptionPackage.Id;
            company.SubscriptionStartTime = DateTime.Now;
            company.SubscriptionEndTime = company.SubscriptionStartTime?.AddDays(subscriptionPackage.DurationInDays);
            company.IsActive = true;
            await _companyRepo.UpdateAsync(company);
            await _companyRepo.SaveAsync();
        }
        public async Task<List<ListSubscriptionPackageDTO>> GetAllSubscriptionPackages()
        {
            var packages = await _subscriptionPackageRepository.GetAllSubscriptionPackages();
            var response = _mapper.Map<List<ListSubscriptionPackageDTO>>(packages);
            return response;
        }

        public async Task<ListSubscriptionPackageDTO> GetSubscriptionPackageById(int id)
        {
            var package = await _subscriptionPackageRepository.GetSubscriptionPackageById(id);
            var response = _mapper.Map<ListSubscriptionPackageDTO>(package);
            return response;
        }

        public async Task CreateSubscriptionPackage(SubscriptionPackageDTO package)
        {
            if (package == null || string.IsNullOrEmpty(package.Name) || package.Price <= 0)
            {
                throw new ArgumentException("All fields must be filled with valid data.");
            }

            var newPackage = _mapper.Map<SubscriptionPackage>(package);
            await _subscriptionPackageRepository.Create(newPackage);
        }

        public async Task UpdateSubscriptionPackage(SubscriptionPackageDTO package, int id)
        {
            if (package == null || string.IsNullOrEmpty(package.Name) || package.Price <= 0)
            {
                throw new ArgumentException("All fields must be filled with valid data.");
            }

            var existingPackage = await _subscriptionPackageRepository.GetSubscriptionPackageById(id);
            if (existingPackage == null)
            {
                throw new ArgumentException("Subscription package not found.");
            }

            var updatedPackage = _mapper.Map<SubscriptionPackage>(package);
            await _subscriptionPackageRepository.Update(updatedPackage, id);
        }

        public async Task DeleteSubscriptionPackage(int id)
        {
            await _subscriptionPackageRepository.Delete(id);
        }
    }
}
