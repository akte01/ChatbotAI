using Chatbot_AI.Data;
using Chatbot_AI.Models;
using Chatbot_AI.Models.Dtos;
using Chatbot_AI.Models.Enums;
using Chatbot_AI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Chatbot_AI.Services;

public class MessageService : IMessageService
{
    private readonly ChatbotAIContext _dbContext;
    private readonly IMessageGeneratorService _messageGeneratorService;
    public MessageService(IMessageGeneratorService messageGeneratorService, ChatbotAIContext dbContext)
    {
        _dbContext = dbContext;
        _messageGeneratorService = messageGeneratorService;
    }

    public async Task<Message> GenerateResponseAsync(UserMessageDto userMessage)
    {
        var response = await _messageGeneratorService.GenerateResponseUsingAiModelAsync(userMessage.Content);
        await SaveUserMessageAsync(userMessage);
        var message = new Message()
        {
            Date = DateTime.UtcNow,
            Content = response,
            Sender = (int)SenderEnum.Chatbot
        };
        var savedMessage = await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
        return savedMessage.Entity;
    }

    public async Task<Message> SaveFeedback(SaveFeedback feedback)
    {
        var message = await _dbContext.Messages.FirstOrDefaultAsync(message => message.MessageId == feedback.MessageId);
        message.Grade = feedback.Grade;
        var savedMessage = _dbContext.Messages.Update(message);
        await _dbContext.SaveChangesAsync();
        return savedMessage.Entity;
    }

    public async Task<Message> CancelResponse(CancelResponseDto responseMessage)
    {
        var message = await _dbContext.Messages.FirstOrDefaultAsync(message => message.MessageId == responseMessage.MessageId);
        message.Content = responseMessage.Content;
        message.Canceled = true;
        var savedMessage = _dbContext.Messages.Update(message);
        await _dbContext.SaveChangesAsync();
        return savedMessage.Entity;
    }

    public async Task<IList<Message>> GetMessagesAsync()
    {
        //TODO: remove take
        return await _dbContext.Messages.OrderByDescending(m => m.Date).Take(10).OrderBy(m => m.Date).ToListAsync();
    }

    private async Task SaveUserMessageAsync(UserMessageDto userMessage)
    {
        var message  = new Message()
        {
            Date = userMessage.Date,
            Content = userMessage.Content,
            Sender = (int)SenderEnum.User
        };

        await _dbContext.Messages.AddAsync(message);
    }
}
