namespace streak.Models
{
    public class WorklogItem
    {
        public long Id { get; set; }
        public string? Summary { get; set; }
        public string? Breakthrough { get; set; }
        public int Complexity { get; set; }
        public bool WasMotivated { get; set; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; set; }
    }
}