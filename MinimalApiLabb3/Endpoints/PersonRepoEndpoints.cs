using MinimalApiLabb3.DTO.PersonDTO;
using MinimalApiLabb3.Repositories;
using static MinimalApiLabb3.Models.PersonModel;

namespace MinimalApiLabb3.Endpoints
{
    public static class PersonRepoEndpoints
    {
        public static void MapPersonRepoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/get-all-persons", async () => await PersonRepository.GetPersonAsync())
               .WithTags("1-Repository Endpoint");

            app.MapGet(pattern: "/get-person-by-id/{personId}", handler: async (int personId) =>
            {
                Person personToReturn = await PersonRepository.GetPersonByIdAsync(personId);
                if (personToReturn != null)
                {
                    return Results.Ok(value: personToReturn);
                }
                else
                {
                    return Results.BadRequest();
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
        }
    }
}
