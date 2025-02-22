using System.ComponentModel.DataAnnotations.Schema;

namespace Chatbot_AI.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public string? Content { get; set; }
        public int? Sender { get; set; }
        public int? Grade { get; set; }
        public bool Canceled { get; set; }
    }
}
