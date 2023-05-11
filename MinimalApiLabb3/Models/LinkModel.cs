using static MinimalApiLabb3.Models.InterestModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinimalApiLabb3.Models
{
    public class LinkModel
    {
        public class Link
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int LinkId { get; set; }

            [Required]
            [StringLength(20)]
            public string LinkTitle { get; set; }

            [Required]
            [StringLength(100)]
            public string URL { get; set; }

            [ForeignKey("Interest")]
            public int? FK_InterestId { get; set; } = null;
            public Interest? Interest { get; set; } // navigation property
        }
    }
}
