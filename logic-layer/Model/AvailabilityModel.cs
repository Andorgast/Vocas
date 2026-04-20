namespace logic_layer
{
    public class AvailabilityModel
    {
        public int Id { get; private set; }
        public string Day { get; private set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public AvailabilityModel(int id, string day, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
