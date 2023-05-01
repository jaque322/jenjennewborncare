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
        [Required]
        public string Type { get; set; }


        
    }
}
