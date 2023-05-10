using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.DTO.PersonDTO
{
    public class PersonGetDTO
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int InterestId { get; set; }
        public string InterestTitle { get; set; }
        public string InterestDescription { get; set; }
    }
}
