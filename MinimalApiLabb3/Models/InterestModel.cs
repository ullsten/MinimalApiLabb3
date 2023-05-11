using static MinimalApiLabb3.Models.PersonModel;
using static MinimalApiLabb3.Program;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static MinimalApiLabb3.Models.LinkModel;

namespace MinimalApiLabb3.Models
{
    public class InterestModel
    {
        public class Interest
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int InterestId { get; set; }

            [Required]
            [StringLength(50)]
            [DisplayName("Title")]
            public string? InterestTitle { get; set; }

            [Required]
            [StringLength(250)]
            [DisplayName("Description")]
            public string? InterestDescription { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Created { get; set; } = DateTime.Now;

            [ForeignKey("Persons")]
            public int? FK_PersonId { get; set; } = null;
            public Person? Persons { get; set; } //navigation
            public ICollection<Link>? Links { get; set; } //navigering
        }
    }
}
