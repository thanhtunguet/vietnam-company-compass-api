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
    [Route("api/businesses")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IBusinessService businessService, ILogger<BusinessController> logger)
        {
            _businessService = businessService ?? throw new ArgumentNullException(nameof(businessService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Business
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessDTO>>> GetBusinesses([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var businesses = await _businessService.GetAllBusinessesAsync(skip, take);
                return Ok(businesses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving businesses");
                return StatusCode(500, "An error occurred while retrieving businesses");
            }
        }

        // GET: api/Business/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessDTO>> GetBusiness(long id)
        {
            try
            {
                var business = await _businessService.GetBusinessByIdAsync(id);
                if (business == null)
                {
                    return NotFound($"Business with ID {id} not found");
                }

                return Ok(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving business with ID {id}");
            }
        }

        // GET: api/Business/code/{code}
        [HttpGet("code/{code}")]
        public async Task<ActionResult<BusinessDTO>> GetBusinessByCode(string code)
        {
            try
            {
                var business = await _businessService.GetBusinessByCodeAsync(code);
                if (business == null)
                {
                    return NotFound($"Business with code {code} not found");
                }

                return Ok(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business with code {Code}", code);
                return StatusCode(500, $"An error occurred while retrieving business with code {code}");
            }
        }

        // GET: api/Business/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountBusinesses()
        {
            try
            {
                var count = await _businessService.CountBusinessesAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting businesses");
                return StatusCode(500, "An error occurred while counting businesses");
            }
        }

        // POST: api/Business
        [HttpPost]
        public async Task<ActionResult<BusinessDTO>> CreateBusiness(BusinessCreateDTO businessDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var business = await _businessService.CreateBusinessAsync(businessDto);
                return CreatedAtAction(nameof(GetBusiness), new { id = business.Id }, business);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating business");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating business");
                return StatusCode(500, "An error occurred while creating business");
            }
        }

        // PUT: api/Business/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BusinessDTO>> UpdateBusiness(long id, BusinessUpdateDTO businessDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var business = await _businessService.UpdateBusinessAsync(id, businessDto);
                if (business == null)
                {
                    return NotFound($"Business with ID {id} not found");
                }

                return Ok(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating business with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating business with ID {id}");
            }
        }

        // DELETE: api/Business/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBusiness(long id)
        {
            try
            {
                var result = await _businessService.DeleteBusinessAsync(id);
                if (!result)
                {
                    return NotFound($"Business with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting business with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting business with ID {id}");
            }
        }
    }
}
