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
    [Route("api/districts")]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;
        private readonly ILogger<DistrictController> _logger;

        public DistrictController(IDistrictService districtService, ILogger<DistrictController> logger)
        {
            _districtService = districtService ?? throw new ArgumentNullException(nameof(districtService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/District
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistrictDTO>>> GetDistricts([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var districts = await _districtService.GetAllDistrictsAsync(skip, take);
                return Ok(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving districts");
                return StatusCode(500, "An error occurred while retrieving districts");
            }
        }

        // GET: api/District/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DistrictDTO>> GetDistrict(long id)
        {
            try
            {
                var district = await _districtService.GetDistrictByIdAsync(id);
                if (district == null)
                {
                    return NotFound($"District with ID {id} not found");
                }

                return Ok(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving district with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving district with ID {id}");
            }
        }

        // GET: api/District/code/{code}
        [HttpGet("code/{code}")]
        public async Task<ActionResult<DistrictDTO>> GetDistrictByCode(string code)
        {
            try
            {
                var district = await _districtService.GetDistrictByCodeAsync(code);
                if (district == null)
                {
                    return NotFound($"District with code {code} not found");
                }

                return Ok(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving district with code {Code}", code);
                return StatusCode(500, $"An error occurred while retrieving district with code {code}");
            }
        }

        // GET: api/District/province/{provinceId}
        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<IEnumerable<DistrictDTO>>> GetDistrictsByProvince(long provinceId)
        {
            try
            {
                var districts = await _districtService.GetDistrictsByProvinceIdAsync(provinceId);
                return Ok(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving districts for province ID {ProvinceId}", provinceId);
                return StatusCode(500, $"An error occurred while retrieving districts for province ID {provinceId}");
            }
        }

        // GET: api/District/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountDistricts([FromQuery] long? provinceId)
        {
            try
            {
                var count = await _districtService.CountDistrictsAsync(provinceId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting districts");
                return StatusCode(500, "An error occurred while counting districts");
            }
        }

        // POST: api/District
        [HttpPost]
        public async Task<ActionResult<DistrictDTO>> CreateDistrict(DistrictCreateDTO districtDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var district = await _districtService.CreateDistrictAsync(districtDto);
                return CreatedAtAction(nameof(GetDistrict), new { id = district.Id }, district);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating district");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating district");
                return StatusCode(500, "An error occurred while creating district");
            }
        }

        // PUT: api/District/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DistrictDTO>> UpdateDistrict(long id, DistrictUpdateDTO districtDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var district = await _districtService.UpdateDistrictAsync(id, districtDto);
                if (district == null)
                {
                    return NotFound($"District with ID {id} not found");
                }

                return Ok(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating district with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating district with ID {id}");
            }
        }

        // DELETE: api/District/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDistrict(long id)
        {
            try
            {
                var result = await _districtService.DeleteDistrictAsync(id);
                if (!result)
                {
                    return NotFound($"District with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting district with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting district with ID {id}");
            }
        }
    }
}
