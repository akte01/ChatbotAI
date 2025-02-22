using Chatbot_AI.Models;
using MediatR;

namespace Chatbot_AI.Queries
{
    public class GetMessagesQuery : IRequest<List<Message>>
    {

    }
}
