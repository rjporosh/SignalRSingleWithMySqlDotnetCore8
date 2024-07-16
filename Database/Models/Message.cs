namespace SignalRSingleWithMySql.Database.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; } // Foreign key to the User table
        public string ReceiverId { get; set; } // Foreign key to the User table
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
