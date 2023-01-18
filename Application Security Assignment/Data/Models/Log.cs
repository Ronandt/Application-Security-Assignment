namespace Application_Security_Assignment.Data.Models
{
    public class Log
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        public string? LoggedEmailUser { get; set; }
        public string? IPAddress { get; set; }
        public string? Action { get; set; }


    }
}
