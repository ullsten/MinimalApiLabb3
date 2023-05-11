using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using MinimalApiLabb3.DTO.LinkDTO;
using static MinimalApiLabb3.Models.LinkModel;
using static MinimalApiLabb3.Program;

namespace MinimalApiLabb3.Endpoints
{
    public static class LinkEndpoints
    {
        public static void MapLinkEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/GetAllLinks", async (Labb3MinmalContext context, [FromQuery] string startsWith = "") =>
            {
                var links = await context.Links
                 .Select(l => new LinkDTO
                 {
                     LinkId = l.LinkId,
                     LinkTitle = l.LinkTitle,
                     URL = l.URL,

                 }).ToListAsync();

                if (links.Count == 0)
                {
                    return Results.NotFound("Sorry, no interests found in database.");
                }

                if (!string.IsNullOrEmpty(startsWith))
                {
                    links = links.Where(l => l.LinkTitle.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (links.Count == 0)
                    {
                        return Results.Ok($"No interests found with a title starting with '{startsWith}'.");
                    }
                }

                return Results.Ok(links);
            });

            app.MapGet("/GetPersonsWithLink", async (Labb3MinmalContext context, [FromQuery] string startsWith = "") =>
            {
                var personLink = await context.Links
                    .Include(l => l.Interest)
                    .Where(l => l.Interest != null)
                    .Select(l => new PersonGetLinkDTO
                    {
                        PersonId = l.Interest.Persons.PersonId,
                        FirstName = l.Interest.Persons.FirstName,
                        LastName = l.Interest.Persons.LastName,
                        LinkId = l.LinkId,
                        LinkTitle = l.LinkTitle,
                        URL = l.URL,
                    })
                    .ToListAsync();


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

            //get by id
            app.MapGet("/GetPersonsWithLink/{id}", async (Labb3MinmalContext context, int id) =>
            {
                var personLink = await context.Links
                    .Include(l => l.Interest)
                    .Where(l => l.Interest.FK_PersonId == id)
                    .Select(l => new PersonGetLinkDTO
                    {

                        PersonId = l.Interest.Persons.PersonId,
                        FirstName = l.Interest.Persons.FirstName,
                        LastName = l.Interest.Persons.LastName,
                        LinkId = l.LinkId,
                        LinkTitle = l.LinkTitle,
                        URL = l.URL,
                    })
                    .ToListAsync();

                if (personLink.Count == 0)
                {
                    return Results.NotFound($"Sorry, no person with ID {id} and links found in database.");
                }

                return Results.Ok(personLink);
            });

            //post new Person link
            app.MapPost("/CreateInterestLink", async (Labb3MinmalContext context, LinkInterestCreateDTO createDTO) =>
            {
                var LinkInterest = new Link
                {
                    LinkId = createDTO.LinkId,
                    LinkTitle = createDTO.LinkTitle,
                    URL = createDTO.URL,
                    FK_InterestId = createDTO.FK_InterestId,
                };

                context.Links.Add(LinkInterest);
                await context.SaveChangesAsync();

                if (await context.Interests.AnyAsync())
                {
                    return Results.Ok(await context.Links.ToListAsync());
                }
                else
                {
                    return Results.NotFound("Sorry, no Interest was added to the database.");
                }
            });

            //Create new link
            app.MapPost("/CreateLink", async (Labb3MinmalContext context, LinkCreateDTO createDto) =>
            {
                var link = new Link
                {
                    LinkTitle = createDto.LinkTitle,
                    URL = createDto.URL
                };

                context.Links.Add(link);
                await context.SaveChangesAsync();

                if (await context.Links.AnyAsync())
                {
                    return Results.Ok(await context.Links.ToListAsync());
                }
                else
                {
                    return Results.Problem("Sorry, no link was created!");
                }
            });

            app.MapPut("/UpdateLinkForPersonInterest/{id}", async (Labb3MinmalContext context, LinkPersonInterestUpdateDTO updateDTO, int id) =>
            {
                var updateLinkPersonInterest = await context.Links.FindAsync(id);

                if (updateLinkPersonInterest is null)
                    return Results.NotFound("Sorry, this links doesn´t exist.");

                updateLinkPersonInterest.LinkTitle = updateDTO.LinkTitle;
                updateLinkPersonInterest.URL = updateDTO.URL;
                await context.SaveChangesAsync();

                return Results.Ok(await context.Links.ToListAsync()); //show response body
            });

            //delete link
            app.MapDelete("/DeleteLink/{id}", async (Labb3MinmalContext context, int id) =>
            {
                var link = await context.Links.FindAsync(id);

                if (link is null)
                    return Results.NotFound($"Sorry, this interest with {id} doesn´t exist");

                context.Links.Remove(link);
                await context.SaveChangesAsync();
                return Results.Ok(await context.Links.ToListAsync());
            });
        }
    }
}
