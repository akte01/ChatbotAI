using Chatbot_AI.Models;
using MediatR;

namespace Chatbot_AI.Commands
{
    public class GenerateResponseCommand : IRequest<Message>
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public GenerateResponseCommand(string content, DateTime date)
        {
            Content = content;
            Date = date;
        }

    }
}
