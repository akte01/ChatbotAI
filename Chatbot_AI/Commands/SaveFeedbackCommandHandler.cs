using Chatbot_AI.Data;
using Chatbot_AI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chatbot_AI.Commands
{
    public class SaveFeedbackCommandHandler : IRequestHandler<SaveFeedbackCommand, Message>
    {
        private readonly ChatbotAIContext _dbContext;

        public SaveFeedbackCommandHandler(ChatbotAIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message> Handle(SaveFeedbackCommand request, CancellationToken cancellationToken)
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(message => message.MessageId == request.MessageId);
            message.Grade = request.Grade;
            var savedMessage = _dbContext.Messages.Update(message);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return savedMessage.Entity;
        }
    }
}
