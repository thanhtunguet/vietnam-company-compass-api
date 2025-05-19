using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;
using VietnamBusiness.Services;

namespace VietnamBusiness.Controllers
{
    [ApiController]
    [Route("api/api-usage-tracking")]
    public class ApiUsageTrackingController : ControllerBase
    {
        private readonly IApiUsageTrackingService _apiUsageTrackingService;
        private readonly ILogger<ApiUsageTrackingController> _logger;

        public ApiUsageTrackingController(IApiUsageTrackingService apiUsageTrackingService, ILogger<ApiUsageTrackingController> logger)
        {
            _apiUsageTrackingService = apiUsageTrackingService ?? throw new ArgumentNullException(nameof(apiUsageTrackingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/ApiUsageTracking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiUsageTrackingDTO>>> GetApiUsageTrackings([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var trackings = await _apiUsageTrackingService.GetAllApiUsageTrackingsAsync(skip, take);
                return Ok(trackings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API usage trackings");
                return StatusCode(500, "An error occurred while retrieving API usage trackings");
            }
        }

        // GET: api/ApiUsageTracking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiUsageTrackingDTO>> GetApiUsageTracking(long id)
        {
            try
            {
                var tracking = await _apiUsageTrackingService.GetApiUsageTrackingByIdAsync(id);
                if (tracking == null)
                {
                    return NotFound($"API usage tracking with ID {id} not found");
                }

                return Ok(tracking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API usage tracking with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving API usage tracking with ID {id}");
            }
        }

        // GET: api/ApiUsageTracking/apikey/{apiKeyId}
        [HttpGet("apikey/{apiKeyId}")]
        public async Task<ActionResult<IEnumerable<ApiUsageTrackingDTO>>> GetApiUsageTrackingsByApiKeyId(long apiKeyId)
        {
            try
            {
                var trackings = await _apiUsageTrackingService.GetApiUsageTrackingsByApiKeyIdAsync(apiKeyId);
                return Ok(trackings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API usage trackings for API key ID {ApiKeyId}", apiKeyId);
                return StatusCode(500, $"An error occurred while retrieving API usage trackings for API key ID {apiKeyId}");
            }
        }

        // GET: api/ApiUsageTracking/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountApiUsageTrackings([FromQuery] long? apiKeyId)
        {
            try
            {
                var count = await _apiUsageTrackingService.CountApiUsageTrackingsAsync(apiKeyId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting API usage trackings");
                return StatusCode(500, "An error occurred while counting API usage trackings");
            }
        }

        // POST: api/ApiUsageTracking
        [HttpPost]
        public async Task<ActionResult<ApiUsageTrackingDTO>> CreateApiUsageTracking(ApiUsageTrackingCreateDTO trackingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var tracking = await _apiUsageTrackingService.CreateApiUsageTrackingAsync(trackingDto);
                return CreatedAtAction(nameof(GetApiUsageTracking), new { id = tracking.Id }, tracking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating API usage tracking");
                return StatusCode(500, "An error occurred while creating API usage tracking");
            }
        }
    }
}
