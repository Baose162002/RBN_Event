using BusinessObject.DTO.ResponseDto;
using BusinessObject.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ISubscriptionPackageService
    {
        Task<List<ListSubscriptionPackageDTO>> GetAllSubscriptionPackages();
        Task<ListSubscriptionPackageDTO> GetSubscriptionPackageById(int id);
        Task CreateSubscriptionPackage(SubscriptionPackageDTO package);
        Task UpdateSubscriptionPackage(SubscriptionPackageDTO package, int id);
        Task DeleteSubscriptionPackage(int id);
    }
}
