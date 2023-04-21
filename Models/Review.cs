using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jenjennewborncare.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }

        // Foreign key for related entity (if any)
        //public int? RelatedEntityId { get; set; }

        // Navigation property for related entity (if any)
       // [ForeignKey("RelatedEntityId")]
        // public virtual RelatedEntity RelatedEntity { get; set; }

    } 
}