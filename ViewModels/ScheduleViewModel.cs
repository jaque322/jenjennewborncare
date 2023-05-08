namespace jenjennewborncare.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string UserId { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}