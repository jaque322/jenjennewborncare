using Google.Apis.PeopleService.v1.Data;
using System.ComponentModel.DataAnnotations;

namespace jenjennewborncare.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}