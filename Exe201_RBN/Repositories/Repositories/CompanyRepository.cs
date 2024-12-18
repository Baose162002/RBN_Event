﻿using BusinessObject;
using BusinessObject.DTO;
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
        public async Task<Company> GetCompanyByEmail(string email)
        {
            using (var _context = new ApplicationDBContext())
            {
                return await _context.Companies
                    .Include(x => x.User)
                    .Include(x => x.SubscriptionPackage)
                    .FirstOrDefaultAsync(x => x.User.Email.ToLower() == email.ToLower());
            }
        }
        public async Task<List<Company>> GetAllCompanyWithSubcription()
        {
            var _context = new ApplicationDBContext();
            var companies = await _context.Companies
                .Include(s=>s.SubscriptionPackage)
                .ToListAsync();
            return companies;

        }
        public async Task<List<Company>> GetAllCompany()
        {
            var _context = new ApplicationDBContext();
            var companies = await _context.Companies.Include(e => e.Events).ToListAsync();
            return companies;

        }
        public async Task<Company> GetCompanyById(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.Companies.Include(e => e.Events).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> GetCompanyByIdUser(int id)
        {
            var _context = new ApplicationDBContext();
            return await _context.Companies.FirstOrDefaultAsync(c => c.UserId == id);
        }
        public async Task Create(Company company)
        {
            var _context = new ApplicationDBContext();
            try
            {
                var existing = await GetCompanyById(company.Id);
                if(existing != null)
                {
                    throw new ArgumentException("Company already exist");
                }
                
                await _context.AddAsync(existing);
                await _context.SaveChangesAsync();
            }catch (Exception e)
            {
                throw e;
            }
        }
        public async Task Update(Company company, int id)
        {
            try
            {
                var _context = new ApplicationDBContext();
                var existing = await GetCompanyById(id);
                if (existing != null)
                {
                    existing.Name = company.Name;
                    existing.Description = company.Description;
                    existing.Address = company.Address;
                    existing.Phone = company.Phone;
                    existing.Avatar = company.Avatar;
                    existing.TaxCode = company.TaxCode;                 
                }
                _context.Companies.Update(existing);
                await _context.SaveChangesAsync();
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
