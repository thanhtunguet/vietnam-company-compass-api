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
    public class CrawlerJobController : ControllerBase
    {
        private readonly ICrawlerJobService _crawlerJobService;
        private readonly ILogger<CrawlerJobController> _logger;

        public CrawlerJobController(ICrawlerJobService crawlerJobService, ILogger<CrawlerJobController> logger)
        {
            _crawlerJobService = crawlerJobService ?? throw new ArgumentNullException(nameof(crawlerJobService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/CrawlerJob
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrawlerJobDTO>>> GetCrawlerJobs([FromQuery] string status, [FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var jobs = await _crawlerJobService.GetAllCrawlerJobsAsync(status, skip, take);
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving crawler jobs");
                return StatusCode(500, "An error occurred while retrieving crawler jobs");
            }
        }

        // GET: api/CrawlerJob/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CrawlerJobDTO>> GetCrawlerJob(Guid id)
        {
            try
            {
                var job = await _crawlerJobService.GetCrawlerJobByIdAsync(id);
                if (job == null)
                {
                    return NotFound($"Crawler job with ID {id} not found");
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving crawler job with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving crawler job with ID {id}");
            }
        }

        // GET: api/CrawlerJob/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountCrawlerJobs([FromQuery] string status)
        {
            try
            {
                var count = await _crawlerJobService.CountCrawlerJobsAsync(status);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting crawler jobs");
                return StatusCode(500, "An error occurred while counting crawler jobs");
            }
        }

        // POST: api/CrawlerJob
        [HttpPost]
        public async Task<ActionResult<CrawlerJobDTO>> CreateCrawlerJob(CrawlerJobCreateDTO jobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var job = await _crawlerJobService.CreateCrawlerJobAsync(jobDto);
                return CreatedAtAction(nameof(GetCrawlerJob), new { id = job.Id }, job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating crawler job");
                return StatusCode(500, "An error occurred while creating crawler job");
            }
        }

        // PUT: api/CrawlerJob/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CrawlerJobDTO>> UpdateCrawlerJob(Guid id, CrawlerJobUpdateDTO jobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var job = await _crawlerJobService.UpdateCrawlerJobAsync(id, jobDto);
                if (job == null)
                {
                    return NotFound($"Crawler job with ID {id} not found");
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating crawler job with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating crawler job with ID {id}");
            }
        }

        // DELETE: api/CrawlerJob/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCrawlerJob(Guid id)
        {
            try
            {
                var result = await _crawlerJobService.DeleteCrawlerJobAsync(id);
                if (!result)
                {
                    return NotFound($"Crawler job with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting crawler job with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting crawler job with ID {id}");
            }
        }
    }
}
