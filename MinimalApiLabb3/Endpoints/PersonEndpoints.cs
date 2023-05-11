using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using MinimalApiLabb3.DTO.PersonDTO;
using static MinimalApiLabb3.Program;

namespace MinimalApiLabb3.Endpoints
{
    public static class PersonEndpoints
    {
        public static void MapPersonEndpoints(this IEndpointRouteBuilder app)
        {
            //get persons with filtered or not
            app.MapGet("/GetAllPerson", async (Labb3MinmalContext context, [FromQuery] string startsWith = "") =>
            {

                var persons = await context.Persons.ToListAsync();
                if (persons.Count == 0)
                {
                    return Results.NotFound("Sorry, no person found in database.");
                }

                if (!string.IsNullOrEmpty(startsWith))
                {
                    persons = persons.Where(p => p.FirstName.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (persons.Count == 0)
                    {
                        return Results.Ok($"No persons found with a first name starting with '{startsWith}'.");
                    }
                }

                return Results.Ok(persons);
            });

            //get by id
            app.MapGet("/GetPerson/{id}", async (Labb3MinmalContext context, int id) =>

                await context.Persons.FindAsync(id) is Person person ?
                Results.Ok(person) :
                Results.NotFound($"Sorry, no person with ID: {id} found"
            ));

            //Create person
            app.MapPost("/CreatePerson", async (Labb3MinmalContext context, PersonCreateDTO createDTO) =>
            {
                var person = new Person
                {
                    FirstName = createDTO.FirstName,
                    LastName = createDTO.LastName,
                    PhoneNumber = createDTO.PhoneNumber,
                };

                context.Persons.Add(person);
                await context.SaveChangesAsync();

                if (await context.Persons.AnyAsync())
                {
                    return Results.Ok(await context.Persons.ToListAsync());
                }
                else
                {
                    return Results.NotFound("Sorry, no person was added to the database.");
                }
            });

            app.MapPut("/UpdatePerson/{id}", async (Labb3MinmalContext context, Person updatedPerson, int id) =>
            {
                var person = await context.Persons.FindAsync(id);

                if (person is null)
                    return Results.NotFound("Sorry, this person doesn´t exist.");

                person.FirstName = updatedPerson.FirstName;
                person.LastName = updatedPerson.LastName;
                person.PhoneNumber = updatedPerson.PhoneNumber;
                await context.SaveChangesAsync();

                return Results.Ok(await context.Persons.ToListAsync());
            });

            //Get person with all interest and link(s) filtered or not
            app.MapGet("/GetPersonInterestLink", async (Labb3MinmalContext context, [FromQuery] string startsWith = "") =>
            {
                var personLink = await context.Links
                 .Include(l => l.Interest)
                 .Where(l => l.FK_InterestId != null) //filter so if no relation to interest, result still get all with relation
                 .Select(l => new PersonInterestLinkGetDTO
                 {
                     PersonId = l.Interest.Persons.PersonId,
                     FirstName = l.Interest.Persons.FirstName,
                     LastName = l.Interest.Persons.LastName,
                     InterestId = l.Interest.InterestId,
                     InterestTitle = l.Interest.InterestTitle,
                     InterestDescription = l.Interest.InterestDescription,
                     LinkId = l.LinkId,
                     LinkTitle = l.LinkTitle,
                     URL = l.URL,
                 }).ToListAsync();

                if (personLink.Count == 0)
                {
                    return Results.NotFound("Sorry, no person with links found in database.");
                }

                if (!string.IsNullOrEmpty(startsWith))
                {
                    personLink = personLink.Where(p => p.FirstName.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (personLink.Count == 0)
                    {
                        return Results.Ok($"No interests found with a title starting with '{startsWith}'.");
                    }
                }

                return Results.Ok(personLink);
            });

            app.MapDelete("/DeletePerson/{id}", async (Labb3MinmalContext context, int id) =>
            {
                var person = await context.Persons.FindAsync(id);

                if (person is null)
                    return Results.NotFound($"Sorry, this person with {id} doesn´t exist");

                context.Persons.Remove(person);
                await context.SaveChangesAsync();
                return Results.Ok(await context.Persons.ToListAsync());
            });
        }
    }
}
