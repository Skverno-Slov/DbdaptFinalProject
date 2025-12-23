using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    /// <summary>
    /// сервис для работы с людьми в БД
    /// </summary>
    public class PersonService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        /// <summary>
        /// получает человека по ФИО, если не находит, создаёт нового.
        /// </summary>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя </param>
        /// <param name="middleName">Отчество (если есть)</param>
        /// <returns>Объект Person (существующий или созданный)</returns>
        public async Task<Person> PostPersonByFullNameAsync(string lastName,
                                                            string firstName,
                                                            string? middleName)
        {
            //Получение человека по ФИО
            var person = await _context.People
                .FirstOrDefaultAsync(p => p.LastName == lastName
                    && p.FirstName == firstName
                    && p.MiddleName == middleName);

            // если найден - возвращает этого человека
            if (person is not null)
                return person;

            //Иначе создает нового
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
