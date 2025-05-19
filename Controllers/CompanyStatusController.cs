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
    [Route("api/company-statuses")]
    public class CompanyStatusController : ControllerBase
    {
        private readonly ICompanyStatusService _companyStatusService;
        private readonly ILogger<CompanyStatusController> _logger;

        public CompanyStatusController(ICompanyStatusService companyStatusService, ILogger<CompanyStatusController> logger)
        {
            _companyStatusService = companyStatusService ?? throw new ArgumentNullException(nameof(companyStatusService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/CompanyStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyStatusDTO>>> GetCompanyStatuses()
        {
            try
            {
                var statuses = await _companyStatusService.GetAllCompanyStatusesAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company statuses");
                return StatusCode(500, "An error occurred while retrieving company statuses");
            }
        }

        // GET: api/CompanyStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyStatusDTO>> GetCompanyStatus(long id)
        {
            try
            {
                var status = await _companyStatusService.GetCompanyStatusByIdAsync(id);
                if (status == null)
                {
                    return NotFound($"Company status with ID {id} not found");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company status with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving company status with ID {id}");
            }
        }

        // GET: api/CompanyStatus/code/{code}
        [HttpGet("code/{code}")]
        public async Task<ActionResult<CompanyStatusDTO>> GetCompanyStatusByCode(string code)
        {
            try
            {
                var status = await _companyStatusService.GetCompanyStatusByCodeAsync(code);
                if (status == null)
                {
                    return NotFound($"Company status with code {code} not found");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company status with code {Code}", code);
                return StatusCode(500, $"An error occurred while retrieving company status with code {code}");
            }
        }

        // POST: api/CompanyStatus
        [HttpPost]
        public async Task<ActionResult<CompanyStatusDTO>> CreateCompanyStatus(CompanyStatusCreateDTO statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var status = await _companyStatusService.CreateCompanyStatusAsync(statusDto);
                return CreatedAtAction(nameof(GetCompanyStatus), new { id = status.Id }, status);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating company status");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company status");
                return StatusCode(500, "An error occurred while creating company status");
            }
        }

        // PUT: api/CompanyStatus/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyStatusDTO>> UpdateCompanyStatus(long id, CompanyStatusUpdateDTO statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var status = await _companyStatusService.UpdateCompanyStatusAsync(id, statusDto);
                if (status == null)
                {
                    return NotFound($"Company status with ID {id} not found");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company status with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating company status with ID {id}");
            }
        }

        // DELETE: api/CompanyStatus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompanyStatus(long id)
        {
            try
            {
                var result = await _companyStatusService.DeleteCompanyStatusAsync(id);
                if (!result)
                {
                    return NotFound($"Company status with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting company status with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting company status with ID {id}");
            }
        }
    }
}
