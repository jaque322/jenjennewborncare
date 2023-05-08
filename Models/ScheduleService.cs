namespace jenjennewborncare.Models
{
    public class ScheduleService
    {
        public int ScheduleItemId { get; set; }
        public Schedule ScheduleItem { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}