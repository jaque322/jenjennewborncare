using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jenjennewborncare.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; }

        [Required]
        [MaxLength(500)]
        public string ServiceContent { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProviderName { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Image> ServiceImages { get; set; }

        public Service()
        {
            DateCreated = DateTime.Now;
            ServiceImages = new HashSet<Image>();
        }
    }
}
