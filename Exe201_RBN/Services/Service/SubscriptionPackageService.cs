using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Repositories.IRepositories;
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
        private readonly IMapper _mapper;

        public SubscriptionPackageService(ISubscriptionPackageRepository subscriptionPackageRepository, IMapper mapper)
        {
            _subscriptionPackageRepository = subscriptionPackageRepository;
            _mapper = mapper;
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
