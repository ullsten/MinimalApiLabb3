using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.DTO.PersonDTO
{
    public class PersonCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
    }
}
