using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class SubscriptionPackageRepository : ISubscriptionPackageRepository
    {
        public async Task<List<SubscriptionPackage>> GetAllSubscriptionPackages()
        {
            var _context = new ApplicationDBContext();
            var packages = await _context.SubscriptionPackage.ToListAsync();
            return packages;
        }

        public async Task<SubscriptionPackage> GetSubscriptionPackageById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.SubscriptionPackage.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Create(SubscriptionPackage package)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetSubscriptionPackageById(package.Id);
                if (existing != null)
                {
                    throw new ArgumentException("SubscriptionPackage already exists");
                }

                await _context.SubscriptionPackage.AddAsync(package);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Update(SubscriptionPackage package, int id)
        {
            try
            {
                var _context = new ApplicationDBContext();
                var existing = await GetSubscriptionPackageById(id);
                if (existing != null)
                {
                    existing.Name = package.Name;
                    existing.Price = package.Price;
                    existing.DurationInDays = package.DurationInDays;
                    existing.Description = package.Description;
                    existing.IsAvailable = package.IsAvailable;

                    _context.SubscriptionPackage.Update(existing);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("SubscriptionPackage not found");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(int id)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetSubscriptionPackageById(id);
                if (existing == null)
                {
                    throw new Exception("SubscriptionPackage not found");
                }

                _context.SubscriptionPackage.Remove(existing);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
