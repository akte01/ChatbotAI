using Chatbot_AI.Entities.Configurations;
using Chatbot_AI.Models;
using Microsoft.EntityFrameworkCore;
namespace Chatbot_AI.Data
{
    public class ChatbotAIContext(DbContextOptions options) : DbContext(options)
  {
    public DbSet<Message> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MessagesEntityTypeConfiguration());
    }
  }
}
