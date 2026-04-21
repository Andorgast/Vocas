namespace data_layer
{
    public class AvailabilityDTO  //record //geen con
    {
        public int Id { get; private set; }
        public string Day { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public AvailabilityDTO(int id, string day, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
