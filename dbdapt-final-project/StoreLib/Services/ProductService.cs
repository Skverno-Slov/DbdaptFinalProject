using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class ProductService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;
    }
}
