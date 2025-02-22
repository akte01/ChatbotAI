using Chatbot_AI.Models;
using MediatR;

namespace Chatbot_AI.Commands
{
    public class SaveFeedbackCommand : IRequest<Message>
    {
        public int MessageId { get; set; }
        public int? Grade { get; set; }

        public SaveFeedbackCommand(int messageId, int? grade)
        {
            MessageId = messageId;
            Grade = grade;
        }

    }
}
