namespace Vocas.ViewModels
{
    public class AvailabilityViewModel
    {
        public string Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public AvailabilityViewModel(string day, TimeOnly startTime, TimeOnly endTime)
        {
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
