using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(seller => seller.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var seller = await _context.Seller.FindAsync(id);
                _context.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }

        }

        public async Task UpdateAsync(Seller seller)
        {
            if( ! await _context.Seller.AnyAsync(obj => obj.Id == seller.Id))
            {
                throw new NotFoundException("Id not found: " + seller.Id);
            }
            try
            {
                _context.Update(seller);

               await _context.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }

    }
}
