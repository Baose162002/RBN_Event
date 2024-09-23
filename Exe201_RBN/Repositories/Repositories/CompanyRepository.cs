using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories 
{
    public class CompanyRepository : ICompanyRepository
    {
        
        public async Task<List<Company>> GetAllCompany()
        {
            var _context = new ApplicationDBContext();
            var companies = await _context.Companies.ToListAsync();
            return companies;

        }
        public async Task<Company> GetCompanyById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task Create(Company company)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetCompanyById(company.Id);
                if(existing != null)
                {
                    throw new Exception("Company already exist");
                }
                _context.Companies.Add(existing);
                await _context.SaveChangesAsync();
            }catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Update(Company company)
        {
            try
            {
                var _context = new ApplicationDBContext();
                var existing = await GetCompanyById(company.Id);
                if(existing == null)
                {
                    throw new Exception("Not company found");
                }
                existing.Id = company.Id;
                existing.Name = company.Name;
                existing.Phone = company.Phone;
                existing.PromotionFees = company.PromotionFees;
                existing.Status = company.Status;
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(int id)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetCompanyById(id);
                if(existing == null)
                {
                    throw new Exception("Delete company not found");
                }
                _context.Companies.Remove(existing);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                throw e;
            }
        }
       
    }
}
