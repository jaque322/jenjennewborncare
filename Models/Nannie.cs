using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace jenjennewborncare.Models
{
    public class Nannie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        public int ImageId { get; set; }
    }
}
