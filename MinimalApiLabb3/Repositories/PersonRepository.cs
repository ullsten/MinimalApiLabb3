﻿
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using MinimalApiLabb3.DTO.PersonDTO;
using static MinimalApiLabb3.Models.InterestModel;
using static MinimalApiLabb3.Models.PersonModel;
using System.Net.Http.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.Json.Serialization;

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
        internal async static Task<List<PersonGetDTO>> GetPersonInterestAsync()
        {
            using (var db = new Labb3MinmalContext())
            {
                var personInterest = await db.Interests
                    .Include(i => i.Persons)
                    .Where(i => i.Persons != null) //filter to not show where interest have no relation to person 
                    .Select(i => new PersonGetDTO
                    {
                        PersonId = i.Persons.PersonId,
                        FirstName = i.Persons.FirstName,
                        LastName = i.Persons.LastName,
                        PhoneNumber = i.Persons.PhoneNumber,
                        InterestId = i.InterestId,
                        InterestTitle = i.InterestTitle,
                        InterestDescription = i.InterestDescription
                    }).ToListAsync();

                return personInterest;
            }
        }

        internal async static Task<Person> GetPersonByIdAsync(int personId)
        {
            using (var db = new Labb3MinmalContext())
            {
                return await db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);
            }
        }

        internal static async Task<bool> CreatePersonAsync(PersonCreateDTO createDTO)
        {
            using (var db = new Labb3MinmalContext())
            {
                    var person = new Person
                    {
                        FirstName = createDTO.FirstName,
                        LastName = createDTO.LastName,
                        PhoneNumber = createDTO.PhoneNumber,
                    };

                    db.Persons.Add(person);
                    await db.SaveChangesAsync();

                
                if (await db.Persons.AnyAsync())
                {
                     Results.Ok(createDTO);
                }
                else
                {
                     Results.NotFound("Sorry, no person was added to database");
                }
                return true;
            }
        }

        internal static async Task<string> UpdatePersonAsync(int PersonId, PersonUpdateDTO updateDTO)
        {
            using (var db = new Labb3MinmalContext())
            {
                try
                {
                    var personToUpdate = await db.Persons.FindAsync(PersonId);

                    if (personToUpdate == null)
                    {
                        return "Person not found";
                    }

                    personToUpdate.FirstName = updateDTO.FirstName;
                    personToUpdate.LastName = updateDTO.LastName;
                    personToUpdate.PhoneNumber = updateDTO.PhoneNumber;

                    await db.SaveChangesAsync();

                    return "Person updated successfully";
                }
                catch (Exception ex)
                {
                    return "Failed to update person";
                }
            }
        }

        internal async static Task<string> DeletePersonAsync(int personId)
        {
            using (var db = new Labb3MinmalContext())
            {
                var person = await db.Persons.FindAsync(personId);

                if (person == null)
                    return $"No person found with ID: ({personId}) to delete";

                db.Persons.Remove(person);
                await db.SaveChangesAsync();

                return $"Person with ID: {personId} removed successfully";
            }
        }


    }
}
