using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ISubscriptionPackageRepository
    {
        Task<List<SubscriptionPackage>> GetAllSubscriptionPackages();
        Task<SubscriptionPackage> GetSubscriptionPackageById(int id);
        Task Create(SubscriptionPackage package);
        Task Update(SubscriptionPackage package, int id);
        Task Delete(int id);
    }
}
