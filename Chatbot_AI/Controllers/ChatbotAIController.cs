using Microsoft.AspNetCore.Mvc;
using Chatbot_AI.Models;
using System.Net;
using MediatR;
using Chatbot_AI.Commands;
using Chatbot_AI.Queries;

namespace Chatbot_AI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatbotAIController : ControllerBase
    {
        private readonly ILogger<ChatbotAIController> _logger;
        private readonly IMediator _mediator;

        public ChatbotAIController(IMediator mediator, ILogger<ChatbotAIController> logger)
        {
            _logger = logger;
            _mediator = mediator;
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
                var results = await _mediator.Send(new GetMessagesQuery());
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
        public async Task<ActionResult<Message>> GenerateResponse([FromBody] GenerateResponseCommand command)
        {
            try
            {
                _logger.LogInformation("Saving user message and generate response...");
                var result = await _mediator.Send(command);
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
        public async Task<ActionResult<Message>> SaveFeedback([FromBody] SaveFeedbackCommand command)
        {
            try
            {
                _logger.LogInformation("Saving response feedback...");
                var result = await _mediator.Send(command);
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
        public async Task<ActionResult<Message>> CancelResponse([FromBody] CancelResponseCommand command)
        {
            try
            {
                _logger.LogInformation("Canceling response...");
                var result = await _mediator.Send(command);
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
