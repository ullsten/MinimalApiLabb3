using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.DTO.LinkDTO
{
    public class PersonLinkUpdateDTO
    {
        [Required]
        [StringLength(50)]
        public string LinkTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string URL { get; set; }

        public int FK_InterestId { get; set; }
    }
}
