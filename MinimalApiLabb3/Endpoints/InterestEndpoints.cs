using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using MinimalApiLabb3.DTO.InterestDTO;
using MinimalApiLabb3.DTO.PersonDTO;
using static MinimalApiLabb3.Models.InterestModel;
using static MinimalApiLabb3.Program;

namespace MinimalApiLabb3.Endpoints
{
    public static class InterestEndpoints
    {
        public static void MapInterestEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/GetAllInterests", async (Labb3MinmalContext context, [FromQuery] string startsWith = "") =>
            {
                var interest = await context.Interests
                 .Select(i => new InterestGetDTO
                 {
                     InterestId = i.InterestId,
                     InterestTitle = i.InterestTitle,
                     InterestDescription = i.InterestDescription,
                     Created = i.Created,

                 }).ToListAsync();

                if (interest.Count == 0)
                {
                    return Results.NotFound("Sorry, no interests found in database.");
                }

                if (!string.IsNullOrEmpty(startsWith))
                {
                    interest = interest.Where(l => l.InterestTitle.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (interest.Count == 0)
                    {
                        return Results.Ok($"No interests found with a title starting with '{startsWith}'.");
                    }
                }

                return Results.Ok(interest);
            }).WithTags("Interest");

            //Routes Interest
            app.MapGet("/GetPersonInterest", async (Labb3MinmalContext context, [FromQuery] string startsWith = "") =>
            {
                var personInterest = await context.Interests
                 .Include(p => p.Persons)
                 .Select(p => new PersonGetDTO
                 {
                     PersonId = p.Persons.PersonId,
                     FirstName = p.Persons.FirstName,
                     LastName = p.Persons.LastName,
                     PhoneNumber = p.Persons.PhoneNumber,
                     InterestId = p.InterestId,
                     InterestTitle = p.InterestTitle,
                     InterestDescription = p.InterestDescription

                 }).ToListAsync();

                if (personInterest.Count == 0)
                {
                    return Results.NotFound("Sorry, no interests found in database.");
                }

                if (!string.IsNullOrEmpty(startsWith))
                {
                    personInterest = personInterest.Where(p => p.FirstName.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (personInterest.Count == 0)
                    {
                        return Results.Ok($"No interests found with a title starting with '{startsWith}'.");
                    }
                }

                return Results.Ok(personInterest);
            }).WithTags("Interest");

            //get by id
            app.MapGet("/PersonInterestBy{id}", async (Labb3MinmalContext context, int id) =>

                await context.Interests.FindAsync(id) is Interest interest ?
                Results.Ok(interest) :
                Results.NotFound($"Sorry, no person with ID: {id} found"
            )).WithTags("Interest");

            //post
            app.MapPost("/CreatePersonInterest", async (Labb3MinmalContext context, PersonInterestCreateDTO createDTO) =>
            {
                var personInterest = new Interest
                {
                    InterestTitle = createDTO.InterestTitle,
                    InterestDescription = createDTO.InterestDescription,
                    FK_PersonId = createDTO.FK_PersonId,
                };

                context.Interests.Add(personInterest);
                await context.SaveChangesAsync();

                if (await context.Interests.AnyAsync())
                {
                    return Results.Ok(await context.Interests.ToListAsync());
                }
                else
                {
                    return Results.NotFound("Sorry, no Interest was added to the database.");
                }
            }).WithTags("Interest");

            app.MapPut("/UpdatePersonInterest/{id}", async (Labb3MinmalContext context, PersonInterestUpdateDTO updateDTO, int id) =>
            {
                var updatePersonInterest = await context.Interests.FindAsync(id);

                if (updatePersonInterest is null)
                    return Results.NotFound("Sorry, this person doesn´t exist.");

                updatePersonInterest.InterestTitle = updateDTO.InterestTitle;
                updatePersonInterest.InterestDescription = updateDTO.InterestDescription;
                updatePersonInterest.FK_PersonId = updateDTO.FK_PersonId;
                await context.SaveChangesAsync();

                return Results.Ok(await context.Interests.ToListAsync());
            }).WithTags("Interest");

            app.MapDelete("/DeletePersonInterest/{id}", async (Labb3MinmalContext context, int id) =>
            {
                var interest = await context.Interests.FindAsync(id);

                if (interest is null)
                    return Results.NotFound($"Sorry, this interest with {id} doesn´t exist");

                context.Interests.Remove(interest);
                await context.SaveChangesAsync();
                return Results.Ok(await context.Interests.ToListAsync());
            }).WithTags("Interest");
        }
            
    }
}
