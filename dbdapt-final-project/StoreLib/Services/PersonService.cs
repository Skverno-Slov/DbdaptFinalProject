using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class PersonService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public async Task<Person> PostPersonByFullNameAsync(string lastName,
                                                            string firstName,
                                                            string? middleName)
        {   
            var person = await _context.People
                .FirstOrDefaultAsync(p => p.LastName == lastName 
                    && p.FirstName == firstName 
                    && p.MiddleName == middleName);

            if (person is not null)
                return person;

            person = new Person()
            {
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName
            };

            await _context.AddAsync(person);
            _context.SaveChanges();

            return person;
        }
    }
}
