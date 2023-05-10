using static MinimalApiLabb3.Program;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.DTO.LinkDTO
{
    public class LinkPersonInterestUpdateDTO
    {
        [Required]
        [StringLength(50)]
        public string LinkTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string URL { get; set; }
    }
}
