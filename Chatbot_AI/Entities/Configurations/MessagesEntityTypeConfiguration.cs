using Chatbot_AI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatbot_AI.Entities.Configurations
{
  public class MessagesEntityTypeConfiguration : IEntityTypeConfiguration<Message>
  {
    public void Configure(EntityTypeBuilder<Message> builder)
    {
      builder.ToTable("Messages");
      builder.HasKey(l => l.MessageId);
            builder.Property(l => l.Date).IsRequired();
      builder.Property(l => l.Content).IsRequired();
      builder.Property(l => l.Grade);
      builder.Property(l => l.Sender).IsRequired();
      builder.Property(l => l.Canceled).HasDefaultValue(false);
    }
  }
}
