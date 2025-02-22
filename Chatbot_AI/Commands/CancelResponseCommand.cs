using Chatbot_AI.Models;
using MediatR;

namespace Chatbot_AI.Commands
{
    public class CancelResponseCommand : IRequest<Message>
    {
        public int MessageId { get; set; }
        public string Content { get; set; }

        public CancelResponseCommand(int messageId, string content)
        {
            MessageId = messageId;
            Content = content;
        }
    }
}
