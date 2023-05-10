using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.DTO.LinkDTO
{
    public class PersonGetLinkDTO
    {
        public int InterestId { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LinkId { get; set; }
        public string LinkTitle { get; set; }

        public string URL { get; set; }
    }
}
