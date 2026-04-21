namespace presentation_layer.Models
{
    public class AvailabilityViewModel
    {
        public int Id { get; private set; }
        public string Day { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public AvailabilityViewModel(int id, string day, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
