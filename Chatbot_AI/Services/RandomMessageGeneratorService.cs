using Chatbot_AI.Services.Interfaces;

namespace Chatbot_AI.Services;

public class RandomMessageGeneratorService : IMessageGeneratorService
{
    public Task<string> GenerateResponseAsync(string userMessageContent)
    {
        var response = string.Empty;
        var breakLine = "<br /><br />";
        Random rnd = new Random();
        int sentenceCount = rnd.Next(1, 5);
        var paragraphCount = rnd.Next(1, 5);
        for (int i = 0; i < paragraphCount; i++)
        {
            var isLast = paragraphCount - i == 1;
            var sentences = string.Join(" ", new NLipsum.Core.LipsumGenerator().GenerateSentences(sentenceCount));
            response += sentences;
            if (!isLast)
            {
                response += breakLine;
            }
        }
        return Task.FromResult(response);
    }

}