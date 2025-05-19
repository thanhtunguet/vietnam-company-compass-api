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
    [Route("api/company-business-mappings")]
    public class CompanyBusinessMappingController : ControllerBase
    {
        private readonly ICompanyBusinessMappingService _mappingService;
        private readonly ILogger<CompanyBusinessMappingController> _logger;

        public CompanyBusinessMappingController(ICompanyBusinessMappingService mappingService, ILogger<CompanyBusinessMappingController> logger)
        {
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/CompanyBusinessMapping
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyBusinessMappingDTO>>> GetAllMappings()
        {
            try
            {
                var mappings = await _mappingService.GetAllMappingsAsync();
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company-business mappings");
                return StatusCode(500, "An error occurred while retrieving company-business mappings");
            }
        }

        // GET: api/CompanyBusinessMapping/company/{companyId}
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<CompanyBusinessMappingDTO>>> GetMappingsByCompany(long companyId)
        {
            try
            {
                var mappings = await _mappingService.GetMappingsByCompanyIdAsync(companyId);
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mappings for company ID {CompanyId}", companyId);
                return StatusCode(500, $"An error occurred while retrieving mappings for company ID {companyId}");
            }
        }

        // GET: api/CompanyBusinessMapping/business/{businessId}
        [HttpGet("business/{businessId}")]
        public async Task<ActionResult<IEnumerable<CompanyBusinessMappingDTO>>> GetMappingsByBusiness(long businessId)
        {
            try
            {
                var mappings = await _mappingService.GetMappingsByBusinessIdAsync(businessId);
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mappings for business ID {BusinessId}", businessId);
                return StatusCode(500, $"An error occurred while retrieving mappings for business ID {businessId}");
            }
        }

        // GET: api/CompanyBusinessMapping/company/{companyId}/business/{businessId}
        [HttpGet("company/{companyId}/business/{businessId}")]
        public async Task<ActionResult<CompanyBusinessMappingDTO>> GetMapping(long companyId, long businessId)
        {
            try
            {
                var mapping = await _mappingService.GetMappingAsync(companyId, businessId);
                if (mapping == null)
                {
                    return NotFound($"Mapping between company ID {companyId} and business ID {businessId} not found");
                }

                return Ok(mapping);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mapping between company ID {CompanyId} and business ID {BusinessId}", companyId, businessId);
                return StatusCode(500, $"An error occurred while retrieving mapping between company ID {companyId} and business ID {businessId}");
            }
        }

        // POST: api/CompanyBusinessMapping
        [HttpPost]
        public async Task<ActionResult<CompanyBusinessMappingDTO>> CreateMapping(CompanyBusinessMappingCreateDTO mappingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var mapping = await _mappingService.CreateMappingAsync(mappingDto);
                return CreatedAtAction(nameof(GetMapping), 
                    new { companyId = mapping.CompanyId, businessId = mapping.BusinessId }, 
                    mapping);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating company-business mapping");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company-business mapping");
                return StatusCode(500, "An error occurred while creating company-business mapping");
            }
        }

        // DELETE: api/CompanyBusinessMapping/company/{companyId}/business/{businessId}
        [HttpDelete("company/{companyId}/business/{businessId}")]
        public async Task<ActionResult> DeleteMapping(long companyId, long businessId)
        {
            try
            {
                var result = await _mappingService.DeleteMappingAsync(companyId, businessId);
                if (!result)
                {
                    return NotFound($"Mapping between company ID {companyId} and business ID {businessId} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting mapping between company ID {CompanyId} and business ID {BusinessId}", companyId, businessId);
                return StatusCode(500, $"An error occurred while deleting mapping between company ID {companyId} and business ID {businessId}");
            }
        }
    }
}
