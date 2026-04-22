namespace data_layer
{
    public record AvailabilityDTO  //record //geen con
    {
        public int Id { get; init; }
        public string Day { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }

        public AvailabilityDTO(int id, string day, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
