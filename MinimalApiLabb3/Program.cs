
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using Microsoft.AspNetCore.Mvc;
using MinimalApiLabb3.DTO.PersonDTO;
using System;
using MinimalApiLabb3.DTO.InterestDTO;
using MinimalApiLabb3.DTO.LinkDTO;

namespace MinimalApiLabb3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<Labb3MinmalContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<Labb3MinmalContext>();
                //check if database exist, if not create database
                context.Database.EnsureCreated();
                //Add fake data to database
                SeedData.Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //Routes

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

            //*************************************************************************

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
            });

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
            });

            //get by id
            app.MapGet("/PersonInterestBy{id}", async (Labb3MinmalContext context, int id) =>

                await context.Interests.FindAsync(id) is Interest interest ?
                Results.Ok(interest) :
                Results.NotFound($"Sorry, no person with ID: {id} found"
            ));

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
            });

            app.MapPut("/UpdatePersonInterest/{id}", async (Labb3MinmalContext context, PersonInterestUpdateDTO updateDTO , int id) =>
            {
                var updatePersonInterest = await context.Interests.FindAsync(id);

                if (updatePersonInterest is null)
                    return Results.NotFound("Sorry, this person doesn´t exist.");

                updatePersonInterest.InterestTitle = updateDTO.InterestTitle;
                updatePersonInterest.InterestDescription = updateDTO.InterestDescription;
                updatePersonInterest.FK_PersonId = updateDTO.FK_PersonId;
                await context.SaveChangesAsync();

                return Results.Ok(await context.Interests.ToListAsync());
            });

            app.MapDelete("/DeletePersonInterest/{id}", async (Labb3MinmalContext context, int id) =>
            {
                var interest = await context.Interests.FindAsync(id);

                if (interest is null)
                    return Results.NotFound($"Sorry, this interest with {id} doesn´t exist");

                context.Interests.Remove(interest);
                await context.SaveChangesAsync();
                return Results.Ok(await context.Interests.ToListAsync());
            });

            //*********************************************************************
            //Routes Links


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

                if(await context.Links.AnyAsync())
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


            app.Run();
        }

        //tables
        public class Person
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int PersonId { get; set; }

            [Required]
            [StringLength(30)]
            [DisplayName("First name")]
            public string? FirstName { get; set; } = default;

            [Required]
            [StringLength(30)]
            [DisplayName("Last name")]
            public string? LastName { get; set; } = default;

            [NotMapped]
            [DisplayName("Person")]
            public string FullName => $"{FirstName} {LastName}";

            [Required]
            [StringLength(20)]
            [DisplayName("Phone number")]
            public string? PhoneNumber { get; set; }

            public ICollection<Interest>? Interests { get; set; } //navigering
        }

        public class Interest
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int InterestId { get; set; }

            [Required]
            [StringLength(50)]
            [DisplayName("Title")]
            public string? InterestTitle { get; set; }

            [Required]
            [StringLength(250)]
            [DisplayName("Description")]
            public string? InterestDescription { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Created { get; set; } = DateTime.Now;

            [ForeignKey("Persons")]
            public int FK_PersonId { get; set; }
            public Person? Persons { get; set; } //navigation
            public ICollection<Link>? Links { get; set; } //navigering
        }

            public class Link
            {
                [Key]
                [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public int LinkId { get; set; }

                [Required]
                [StringLength(20)]
                public string LinkTitle { get; set; }

                [Required]
                [StringLength(100)]
                public string URL { get; set; }

                [ForeignKey("Interest")]
                public int? FK_InterestId { get; set; } = null;
                public Interest Interest { get; set; } // navigation property
            }
    }
}
