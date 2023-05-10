using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.DTO.LinkDTO
{
    public class LinkCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string LinkTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string URL { get; set; }
    }
}
