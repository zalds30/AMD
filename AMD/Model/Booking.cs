namespace AMD.Model
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string Venue { get; set; }
        public string Package { get; set; }
        public string ContactNumber { get; set; }
        public decimal EstimatedPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string RawMessage { get; set; }
    }
}
