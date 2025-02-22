using Microsoft.AspNetCore.Mvc;
using Chatbot_AI.Models;
using Chatbot_AI.Services.Interfaces;
using Chatbot_AI.Models.Dtos;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace Chatbot_AI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatbotAIController : ControllerBase
    {
        private readonly ILogger<ChatbotAIController> _logger;
        private readonly IMessageService _messageService;

        public ChatbotAIController(IMessageService messageService, ILogger<ChatbotAIController> logger)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("GetMessages")]
        [ProducesResponseType(typeof(List<Message>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<Message>>> GetChatHistory()
        {
            try
            {
                _logger.LogInformation("Getting chat history...");
                var results = await _messageService.GetMessagesAsync();
                return results.Any() ? Ok(results) : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not retrieved chat history.");
                return NotFound();
            }
        }

        [HttpPost]
        [Route("GenerateResponse")]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Message>> GenerateResponse([FromBody] UserMessageDto message)
        {
            try
            {
                _logger.LogInformation("Saving user message and generate response...");
                var result = await _messageService.GenerateResponseAsync(message);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Saving user message and generate response failed.");
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("SaveFeedback")]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Message>> SaveFeedback([FromBody] SaveFeedback feedback)
        {
            try
            {
                _logger.LogInformation("Saving response feedback...");
                var result = await _messageService.SaveFeedback(feedback);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Saving response feedback failed.");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CancelResponse")]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Message>> CancelResponse([FromBody] CancelResponseDto message)
        {
            try
            {
                _logger.LogInformation("Canceling response...");
                var result = await _messageService.CancelResponse(message);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Canceling response failed.");
                return BadRequest();
            }
        }
    }
}
