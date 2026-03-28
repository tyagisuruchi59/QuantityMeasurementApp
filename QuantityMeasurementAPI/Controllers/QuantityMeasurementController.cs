using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModel.DTOs;

namespace QuantityMeasurementAPI.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    [Authorize]
    [Produces("application/json")]
    [SwaggerTag("Quantity Measurements - REST API for quantity measurement operations")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;
        private readonly ILogger<QuantityMeasurementController> _logger;

        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            ILogger<QuantityMeasurementController> logger)
        {
            _service = service;
            _logger  = logger;
        }

        // ─── POST /compare ────────────────────────────────────────────────────

        [HttpPost("compare")]
        [SwaggerOperation(Summary = "Compare two quantities", OperationId = "CompareQuantities")]
        [SwaggerResponse(200, "Comparison result", typeof(QuantityMeasurementDTO))]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(401, "Unauthorized")]
        public async Task<IActionResult> CompareQuantities([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /compare called by {User}", User.Identity?.Name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CompareAsync(input);

            if (result.IsError)
                return BadRequest(new { result.ErrorMessage, result.Operation });

            return Ok(result);
        }

        // ─── POST /convert ────────────────────────────────────────────────────

        [HttpPost("convert")]
        [SwaggerOperation(Summary = "Convert a quantity to a different unit", OperationId = "ConvertQuantity")]
        [SwaggerResponse(200, "Conversion result", typeof(QuantityMeasurementDTO))]
        [SwaggerResponse(400, "Invalid input or incompatible units")]
        public async Task<IActionResult> ConvertQuantity([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /convert called");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ConvertAsync(input);

            if (result.IsError)
                return BadRequest(new { result.ErrorMessage, result.Operation });

            return Ok(result);
        }

        // ─── POST /add ────────────────────────────────────────────────────────

        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add two quantities", OperationId = "AddQuantities")]
        [SwaggerResponse(200, "Sum result", typeof(QuantityMeasurementDTO))]
        [SwaggerResponse(400, "Invalid input or incompatible categories")]
        public async Task<IActionResult> AddQuantities([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /add called");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(input);

            if (result.IsError)
                return BadRequest(new { result.ErrorMessage, result.Operation });

            return Ok(result);
        }

        // ─── POST /subtract ───────────────────────────────────────────────────

        [HttpPost("subtract")]
        [SwaggerOperation(Summary = "Subtract two quantities", OperationId = "SubtractQuantities")]
        [SwaggerResponse(200, "Difference result", typeof(QuantityMeasurementDTO))]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<IActionResult> SubtractQuantities([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /subtract called");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.SubtractAsync(input);

            if (result.IsError)
                return BadRequest(new { result.ErrorMessage, result.Operation });

            return Ok(result);
        }

        // ─── POST /divide ─────────────────────────────────────────────────────

        [HttpPost("divide")]
        [SwaggerOperation(Summary = "Divide two quantities", OperationId = "DivideQuantities")]
        [SwaggerResponse(200, "Division result", typeof(QuantityMeasurementDTO))]
        [SwaggerResponse(400, "Division by zero or incompatible categories")]
        public async Task<IActionResult> DivideQuantities([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("POST /divide called");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DivideAsync(input);

            if (result.IsError)
                return BadRequest(new { result.ErrorMessage, result.Operation });

            return Ok(result);
        }

        // ─── GET /history/operation/{operation} ───────────────────────────────

        [HttpGet("history/operation/{operation}")]
        [SwaggerOperation(Summary = "Get measurement history by operation", OperationId = "GetOperationHistory")]
        [SwaggerResponse(200, "List of measurements", typeof(List<QuantityMeasurementDTO>))]
        public async Task<IActionResult> GetOperationHistory([FromRoute] string operation)
        {
            _logger.LogInformation("GET /history/operation/{Operation}", operation);
            var result = await _service.GetHistoryByOperationAsync(operation);
            return Ok(result);
        }

        // ─── GET /history/type/{measurementType} ──────────────────────────────

        [HttpGet("history/type/{measurementType}")]
        [SwaggerOperation(Summary = "Get measurement history by type", OperationId = "GetTypeHistory")]
        [SwaggerResponse(200, "List of measurements", typeof(List<QuantityMeasurementDTO>))]
        public async Task<IActionResult> GetTypeHistory([FromRoute] string measurementType)
        {
            _logger.LogInformation("GET /history/type/{Type}", measurementType);
            var result = await _service.GetHistoryByMeasurementTypeAsync(measurementType);
            return Ok(result);
        }

        // ─── GET /history/errored ─────────────────────────────────────────────

        [HttpGet("history/errored")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Get errored measurements (Admin only)", OperationId = "GetErroredMeasurements")]
        [SwaggerResponse(200, "List of errored measurements", typeof(List<QuantityMeasurementDTO>))]
        [SwaggerResponse(403, "Forbidden - Admin role required")]
        public async Task<IActionResult> GetErroredMeasurements()
        {
            _logger.LogInformation("GET /history/errored called");
            var result = await _service.GetErroredMeasurementsAsync();
            return Ok(result);
        }

        // ─── GET /count/{operation} ───────────────────────────────────────────

        [HttpGet("count/{operation}")]
        [SwaggerOperation(Summary = "Get operation count", OperationId = "GetOperationCount")]
        [SwaggerResponse(200, "Count of operations")]
        public async Task<IActionResult> GetOperationCount([FromRoute] string operation)
        {
            _logger.LogInformation("GET /count/{Operation}", operation);
            var count = await _service.GetOperationCountAsync(operation);
            return Ok(new { operation = operation.ToUpper(), count });
        }
    }
}