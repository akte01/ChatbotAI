using Chatbot_AI.Models;

namespace Chatbot_AI.Services.Interfaces;

public interface IMessageGeneratorService
{
  public Task<string> GenerateResponseUsingAiModelAsync(string userMessageContent);

}
