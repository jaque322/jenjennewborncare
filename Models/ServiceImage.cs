using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jenjennewborncare.Models
{
    public class ServiceImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [ForeignKey("Service")]
        public int BabyCareServiceModelId { get; set; }
        public virtual Service? BabyCareServiceModel { get; set; }
    }
}
