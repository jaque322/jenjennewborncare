namespace jenjennewborncare.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<ScheduleService> ScheduleServices { get; set; }
    }
}
