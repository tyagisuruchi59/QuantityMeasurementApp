using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qma_service.Models;
using qma_service.Services;

namespace qma_service.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    [Authorize]
    [Produces("application/json")]
    public class QuantityController : ControllerBase
    {
        private readonly QmaService _service;

        public QuantityController(QmaService service)
        {
            _service = service;
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Compare([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CompareAsync(input);
            return result.IsError ? BadRequest(result) : Ok(result);
        }

        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.ConvertAsync(input);
            return result.IsError ? BadRequest(result) : Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.AddAsync(input);
            return result.IsError ? BadRequest(result) : Ok(result);
        }

        [HttpPost("subtract")]
        public async Task<IActionResult> Subtract([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.SubtractAsync(input);
            return result.IsError ? BadRequest(result) : Ok(result);
        }

        [HttpPost("divide")]
        public async Task<IActionResult> Divide([FromBody] QuantityInputDTO input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.DivideAsync(input);
            return result.IsError ? BadRequest(result) : Ok(result);
        }

        [HttpGet("history/type/{type}")]
        public async Task<IActionResult> GetByType([FromRoute] string type)
        {
            var result = await _service.GetHistoryByTypeAsync(type);
            return Ok(result);
        }

        [HttpGet("history/operation/{operation}")]
        public async Task<IActionResult> GetByOperation([FromRoute] string operation)
        {
            var result = await _service.GetHistoryByOperationAsync(operation);
            return Ok(result);
        }
    }
}