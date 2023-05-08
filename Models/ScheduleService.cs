namespace jenjennewborncare.Models
{
    public class ScheduleService
    {
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}