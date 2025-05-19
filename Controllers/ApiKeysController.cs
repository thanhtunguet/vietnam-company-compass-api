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
    [Route("api/api-keys")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly ILogger<ApiKeysController> _logger;

        public ApiKeysController(IApiKeyService apiKeyService, ILogger<ApiKeysController> logger)
        {
            _apiKeyService = apiKeyService ?? throw new ArgumentNullException(nameof(apiKeyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/ApiKeys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiKeyDTO>>> GetApiKeys([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var apiKeys = await _apiKeyService.GetAllApiKeysAsync(skip, take);
                return Ok(apiKeys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API keys");
                return StatusCode(500, "An error occurred while retrieving API keys");
            }
        }

        // GET: api/ApiKeys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiKeyDTO>> GetApiKey(long id)
        {
            try
            {
                var apiKey = await _apiKeyService.GetApiKeyByIdAsync(id);
                if (apiKey == null)
                {
                    return NotFound($"API key with ID {id} not found");
                }

                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API key with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving API key with ID {id}");
            }
        }

        // GET: api/ApiKeys/key/{key}
        [HttpGet("key/{key}")]
        public async Task<ActionResult<ApiKeyDTO>> GetApiKeyByKey(string key)
        {
            try
            {
                var apiKey = await _apiKeyService.GetApiKeyByKeyAsync(key);
                if (apiKey == null)
                {
                    return NotFound($"API key {key} not found");
                }

                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API key {Key}", key);
                return StatusCode(500, $"An error occurred while retrieving API key {key}");
            }
        }

        // GET: api/ApiKeys/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ApiKeyDTO>>> GetApiKeysByUserId(long userId)
        {
            try
            {
                var apiKeys = await _apiKeyService.GetApiKeysByUserIdAsync(userId);
                return Ok(apiKeys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API keys for user ID {UserId}", userId);
                return StatusCode(500, $"An error occurred while retrieving API keys for user ID {userId}");
            }
        }

        // POST: api/ApiKeys
        [HttpPost]
        public async Task<ActionResult<ApiKeyDTO>> CreateApiKey(ApiKeyCreateDTO apiKeyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var apiKey = await _apiKeyService.CreateApiKeyAsync(apiKeyDto);
                return CreatedAtAction(nameof(GetApiKey), new { id = apiKey.Id }, apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating API key");
                return StatusCode(500, "An error occurred while creating API key");
            }
        }

        // PUT: api/ApiKeys/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiKeyDTO>> UpdateApiKey(long id, ApiKeyUpdateDTO apiKeyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var apiKey = await _apiKeyService.UpdateApiKeyAsync(id, apiKeyDto);
                if (apiKey == null)
                {
                    return NotFound($"API key with ID {id} not found");
                }

                return Ok(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating API key with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating API key with ID {id}");
            }
        }

        // DELETE: api/ApiKeys/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteApiKey(long id)
        {
            try
            {
                var result = await _apiKeyService.DeleteApiKeyAsync(id);
                if (!result)
                {
                    return NotFound($"API key with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting API key with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting API key with ID {id}");
            }
        }

        // POST: api/ApiKeys/validate
        [HttpPost("validate")]
        public async Task<ActionResult<bool>> ValidateApiKey([FromBody] string key)
        {
            try
            {
                var isValid = await _apiKeyService.ValidateApiKeyAsync(key);
                return Ok(isValid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating API key");
                return StatusCode(500, "An error occurred while validating API key");
            }
        }
    }
}
