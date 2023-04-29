using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jenjennewborncare.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        public int Type { get; set; }

        //[ForeignKey("Service")]
        //public int BabyCareServiceModelId { get; set; }
        //public virtual Service? BabyCareServiceModel { get; set; }
    }
}
