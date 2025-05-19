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
    [Route("api/[controller]")]
    public class CrawlingStatusController : ControllerBase
    {
        private readonly ICrawlingStatusService _crawlingStatusService;
        private readonly ILogger<CrawlingStatusController> _logger;

        public CrawlingStatusController(ICrawlingStatusService crawlingStatusService, ILogger<CrawlingStatusController> logger)
        {
            _crawlingStatusService = crawlingStatusService ?? throw new ArgumentNullException(nameof(crawlingStatusService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/CrawlingStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrawlingStatusDTO>>> GetCrawlingStatuses()
        {
            try
            {
                var statuses = await _crawlingStatusService.GetAllCrawlingStatusesAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving crawling statuses");
                return StatusCode(500, "An error occurred while retrieving crawling statuses");
            }
        }

        // GET: api/CrawlingStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CrawlingStatusDTO>> GetCrawlingStatus(long id)
        {
            try
            {
                var status = await _crawlingStatusService.GetCrawlingStatusByIdAsync(id);
                if (status == null)
                {
                    return NotFound($"Crawling status with ID {id} not found");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving crawling status with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving crawling status with ID {id}");
            }
        }

        // GET: api/CrawlingStatus/code/{code}
        [HttpGet("code/{code}")]
        public async Task<ActionResult<CrawlingStatusDTO>> GetCrawlingStatusByCode(string code)
        {
            try
            {
                var status = await _crawlingStatusService.GetCrawlingStatusByCodeAsync(code);
                if (status == null)
                {
                    return NotFound($"Crawling status with code {code} not found");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving crawling status with code {Code}", code);
                return StatusCode(500, $"An error occurred while retrieving crawling status with code {code}");
            }
        }

        // POST: api/CrawlingStatus
        [HttpPost]
        public async Task<ActionResult<CrawlingStatusDTO>> CreateCrawlingStatus(CrawlingStatusCreateDTO statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var status = await _crawlingStatusService.CreateCrawlingStatusAsync(statusDto);
                return CreatedAtAction(nameof(GetCrawlingStatus), new { id = status.Id }, status);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating crawling status");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating crawling status");
                return StatusCode(500, "An error occurred while creating crawling status");
            }
        }

        // PUT: api/CrawlingStatus/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CrawlingStatusDTO>> UpdateCrawlingStatus(long id, CrawlingStatusUpdateDTO statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var status = await _crawlingStatusService.UpdateCrawlingStatusAsync(id, statusDto);
                if (status == null)
                {
                    return NotFound($"Crawling status with ID {id} not found");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating crawling status with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating crawling status with ID {id}");
            }
        }

        // DELETE: api/CrawlingStatus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCrawlingStatus(long id)
        {
            try
            {
                var result = await _crawlingStatusService.DeleteCrawlingStatusAsync(id);
                if (!result)
                {
                    return NotFound($"Crawling status with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting crawling status with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting crawling status with ID {id}");
            }
        }
    }
}
