using Chatbot_AI.Data;
using Chatbot_AI.Models;
using Chatbot_AI.Models.Enums;
using Chatbot_AI.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot_AI.Commands
{
    public class CancelResponseCommandHandler : IRequestHandler<CancelResponseCommand, Message>
    {
        private readonly ChatbotAIContext _dbContext;

        public CancelResponseCommandHandler(ChatbotAIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message> Handle(CancelResponseCommand request, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(message => message.MessageId == request.MessageId);
            message.Content = request.Content;
            message.Canceled = true;
            var savedMessage = _dbContext.Messages.Update(message);
            await _dbContext.SaveChangesAsync();
            return savedMessage.Entity;
        }
    }
}
