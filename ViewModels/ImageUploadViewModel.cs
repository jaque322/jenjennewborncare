using jenjennewborncare.Models;

namespace jenjennewborncare.ViewModels
{
    public class ImageUploadViewModel
    {
        public int Id { get; set; } // Add this line to include the Id property
        public string Title { get; set; }
        public IFormFile FileToUpload { get; set; }
        public string TypeId { get; set; } // Change this line to use an int property for TypeId

    }
}
