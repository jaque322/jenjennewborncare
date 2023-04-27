using System.ComponentModel.DataAnnotations;

namespace jenjennewborncare.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "YouTube video ID must be 11 characters long.")]
        public string? YouTubeVideoId { get; set; }
    }
}
