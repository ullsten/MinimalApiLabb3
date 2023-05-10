using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MinimalApiLabb3.DTO.InterestDTO
{
    public class PersonInterestCreateDTO
    {
        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string? InterestTitle { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Description")]
        public string? InterestDescription { get; set; }

        public int FK_PersonId { get; set; }
    }
}
