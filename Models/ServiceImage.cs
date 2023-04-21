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

        [ForeignKey("BabyCareServiceModel")]
        public int BabyCareServiceModelId { get; set; }
        public virtual BabyCareServiceModel? BabyCareServiceModel { get; set; }
    }
}
