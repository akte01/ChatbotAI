using Chatbot_AI.Data;
using Chatbot_AI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot_AI.Queries
{
    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<Message>>
    {
        private readonly ChatbotAIContext _dbContext;

        public GetMessagesQueryHandler(ChatbotAIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Messages.ToListAsync();
        }
    }
}
