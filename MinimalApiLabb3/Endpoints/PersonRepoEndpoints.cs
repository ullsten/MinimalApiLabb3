using MinimalApiLabb3.Data;
using MinimalApiLabb3.DTO.PersonDTO;
using MinimalApiLabb3.Repositories;
using static MinimalApiLabb3.Models.PersonModel;

namespace MinimalApiLabb3.Endpoints
{
    public static class PersonRepoEndpoints
    {
        public static void MapPersonRepoEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/get-Persons-With-Interest", async () => await PersonRepository.GetPersonInterestAsync())
            .WithTags("1-Repository Endpoint");



            app.MapGet("/get-all-persons", async () => await PersonRepository.GetPersonAsync())
               .WithTags("1-Repository Endpoint");

            //Get person by id
            app.MapGet(pattern: "/get-person-by-id/{personId}", handler: async (int personId) =>
            {
                if (personId == 0)
                {
                    return Results.BadRequest("Person ID cannot be zero.");
                }

                if (personId < 0)
                {
                    return Results.BadRequest("Person ID cannot be negative.");
                }

                Person personToReturn = await PersonRepository.GetPersonByIdAsync(personId);
                if (personToReturn != null)
                {
                    return Results.Ok(personToReturn);
                }
                else
                {
                    return Results.BadRequest($"No person found with ID: {personId}");
                }
            }).WithTags("1-Repository Endpoint");



            app.MapPost(pattern: "/create-person", handler: async (PersonCreateDTO createDTO) =>
            {
                bool createSuccessful = await PersonRepository.CreatePersonAsync(createDTO);
                if (createSuccessful)
                {
                    return Results.Ok(value: "Person Createed successfully");
                }
                else
                {
                    return Results.BadRequest();
                }
            }).WithTags("1-Repository Endpoint");


            app.MapPut("/update-person/{personId}", async (int personId, PersonUpdateDTO updateDTO) =>
            {
                string message = await PersonRepository.UpdatePersonAsync(personId, updateDTO);
                if (message == "Person updated successfully")
                {
                    return Results.Ok(message);
                }
                else
                {
                    return Results.BadRequest(message);
                }
            }).WithTags("1-Repository Endpoint");


            app.MapDelete("/delete-person-by-id/{personId}", async (int personId) =>
            {
                string deleteResult = await PersonRepository.DeletePersonAsync(personId);

                if (deleteResult == ($"Person with ID: {personId} removed successfully"))
                {
                    return Results.Ok(deleteResult);
                }
                else
                {
                    return Results.BadRequest(deleteResult);
                }
            }).WithTags("1-Repository Endpoint");

        }
    }
}
