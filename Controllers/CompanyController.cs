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
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompanies([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync(skip, take);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving companies");
                return StatusCode(500, "An error occurred while retrieving companies");
            }
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDTO>> GetCompany(long id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);
                if (company == null)
                {
                    return NotFound($"Company with ID {id} not found");
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving company with ID {id}");
            }
        }

        // GET: api/Company/taxcode/{taxCode}
        [HttpGet("taxcode/{taxCode}")]
        public async Task<ActionResult<CompanyDTO>> GetCompanyByTaxCode(string taxCode)
        {
            try
            {
                var company = await _companyService.GetCompanyByTaxCodeAsync(taxCode);
                if (company == null)
                {
                    return NotFound($"Company with tax code {taxCode} not found");
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company with tax code {TaxCode}", taxCode);
                return StatusCode(500, $"An error occurred while retrieving company with tax code {taxCode}");
            }
        }

        // GET: api/Company/province/{provinceId}
        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompaniesByProvince(long provinceId, [FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var companies = await _companyService.GetCompaniesByProvinceIdAsync(provinceId, skip, take);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving companies for province ID {ProvinceId}", provinceId);
                return StatusCode(500, $"An error occurred while retrieving companies for province ID {provinceId}");
            }
        }

        // GET: api/Company/district/{districtId}
        [HttpGet("district/{districtId}")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompaniesByDistrict(long districtId, [FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var companies = await _companyService.GetCompaniesByDistrictIdAsync(districtId, skip, take);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving companies for district ID {DistrictId}", districtId);
                return StatusCode(500, $"An error occurred while retrieving companies for district ID {districtId}");
            }
        }

        // GET: api/Company/ward/{wardId}
        [HttpGet("ward/{wardId}")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompaniesByWard(long wardId, [FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var companies = await _companyService.GetCompaniesByWardIdAsync(wardId, skip, take);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving companies for ward ID {WardId}", wardId);
                return StatusCode(500, $"An error occurred while retrieving companies for ward ID {wardId}");
            }
        }

        // GET: api/Company/status/{statusId}
        [HttpGet("status/{statusId}")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompaniesByStatus(long statusId, [FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var companies = await _companyService.GetCompaniesByStatusIdAsync(statusId, skip, take);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving companies for status ID {StatusId}", statusId);
                return StatusCode(500, $"An error occurred while retrieving companies for status ID {statusId}");
            }
        }

        // GET: api/Company/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountCompanies(
            [FromQuery] long? provinceId, 
            [FromQuery] long? districtId, 
            [FromQuery] long? wardId, 
            [FromQuery] long? statusId)
        {
            try
            {
                var count = await _companyService.CountCompaniesAsync(provinceId, districtId, wardId, statusId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting companies");
                return StatusCode(500, "An error occurred while counting companies");
            }
        }

        // POST: api/Company
        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> CreateCompany(CompanyCreateDTO companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = await _companyService.CreateCompanyAsync(companyDto);
                return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating company");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company");
                return StatusCode(500, "An error occurred while creating company");
            }
        }

        // PUT: api/Company/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDTO>> UpdateCompany(long id, CompanyUpdateDTO companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = await _companyService.UpdateCompanyAsync(id, companyDto);
                if (company == null)
                {
                    return NotFound($"Company with ID {id} not found");
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating company with ID {id}");
            }
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(long id)
        {
            try
            {
                var result = await _companyService.DeleteCompanyAsync(id);
                if (!result)
                {
                    return NotFound($"Company with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting company with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting company with ID {id}");
            }
        }
    }
}
