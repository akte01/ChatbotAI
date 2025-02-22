using Chatbot_AI.Models;
using Chatbot_AI.Models.Dtos;

namespace Chatbot_AI.Services.Interfaces;

public interface IMessageService
{
  public Task<IList<Message>> GetMessagesAsync();
  public Task<Message> GenerateResponseAsync(UserMessageDto userMessage);
  public Task<Message> SaveFeedback(SaveFeedback feedback);
  public Task<Message> CancelResponse(CancelResponseDto responseMessage);
}
