using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class ManufacturerService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public async Task<List<Manufacturer>> GetManufacturersAsync()
            => await _context.Manufacturers
            .ToListAsync();
    }
}
