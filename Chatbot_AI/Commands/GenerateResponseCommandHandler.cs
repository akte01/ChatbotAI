using Chatbot_AI.Data;
using Chatbot_AI.Models;
using Chatbot_AI.Models.Enums;
using Chatbot_AI.Services.Interfaces;
using MediatR;

namespace Chatbot_AI.Commands
{
    public class GenerateResponseCommandHandler : IRequestHandler<GenerateResponseCommand, Message>
    {
        private readonly ChatbotAIContext _dbContext;
        private readonly IMessageGeneratorService _messageGeneratorService;

        public GenerateResponseCommandHandler(IMessageGeneratorService messageGeneratorService, ChatbotAIContext dbContext)
        {
            _dbContext = dbContext;
            _messageGeneratorService = messageGeneratorService;
        }

        public async Task<Message> Handle(GenerateResponseCommand request, CancellationToken cancellationToken)
        {
            var response = await _messageGeneratorService.GenerateResponseAsync(request.Content);
            await SaveUserMessageAsync(request);
            var message = new Message()
            {
                Date = DateTime.UtcNow,
                Content = response,
                Sender = (int)SenderEnum.Chatbot
            };
            var savedMessage = await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return savedMessage.Entity;
        }

        private async Task SaveUserMessageAsync(GenerateResponseCommand request)
        {
            var message = new Message()
            {
                Date = request.Date,
                Content = request.Content,
                Sender = (int)SenderEnum.User
            };

            await _dbContext.Messages.AddAsync(message);
        }
    }
}
