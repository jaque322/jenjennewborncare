using jenjennewborncare.Areas.Identity.Data;

namespace jenjennewborncare.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<ScheduleService> ScheduleServices { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
