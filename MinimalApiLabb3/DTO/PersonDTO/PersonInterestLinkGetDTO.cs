namespace MinimalApiLabb3.DTO.PersonDTO
{
    public class PersonInterestLinkGetDTO
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int InterestId { get; set; }
        public string InterestTitle { get; set; }
        public string InterestDescription { get; set; }
        public int LinkId { get; set; }
        public string LinkTitle { get; set; }

        public string URL { get; set; }
    }
}
