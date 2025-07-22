namespace DailyPlanner.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public required string Level { get; set; }
        public required string Message { get; set; }
        public string? Exception { get; set; }
        public string? ActorId { get; set; }
    }
}