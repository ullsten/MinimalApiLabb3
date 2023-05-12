using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using MinimalApiLabb3.DTO.PersonDTO;
using static MinimalApiLabb3.Models.PersonModel;

namespace MinimalApiLabb3.Repositories
{
    public class PersonRepository
    {
        internal async static Task<List<Person>> GetPersonAsync()
        {
            using (var db = new Labb3MinmalContext())
            {
                return await db.Persons.ToListAsync();
            }
        }

        internal async static Task<Person> GetPersonByIdAsync(int personId)
        {
            using (var db = new Labb3MinmalContext())
            {
                return await db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);
            }
        }

        internal async static Task<bool> CreatePersonAsync (PersonCreateDTO createDTO)
        {
            using(var  db = new Labb3MinmalContext())
            {
                var person = new Person
                {
                    FirstName = createDTO.FirstName,
                    LastName = createDTO.LastName,
                    PhoneNumber = createDTO.PhoneNumber,
                };

                db.Persons.Add(person);
                await db.SaveChangesAsync();
            }
        

            //if(await dbContext.Persons.AnyAsync())
            //{
            //    return Results.Ok(await dbContext.Persons.ToListAsync());
            //}
            //else
            //{
            //    return Results.NotFound(person);
            //}
            return true;
        }
    }
}
