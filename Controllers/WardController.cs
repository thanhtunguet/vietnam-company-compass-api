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
    [Route("api/wards")]
    public class WardController : ControllerBase
    {
        private readonly IWardService _wardService;
        private readonly ILogger<WardController> _logger;

        public WardController(IWardService wardService, ILogger<WardController> logger)
        {
            _wardService = wardService ?? throw new ArgumentNullException(nameof(wardService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Ward
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WardDTO>>> GetWards([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var wards = await _wardService.GetAllWardsAsync(skip, take);
                return Ok(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wards");
                return StatusCode(500, "An error occurred while retrieving wards");
            }
        }

        // GET: api/Ward/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WardDTO>> GetWard(long id)
        {
            try
            {
                var ward = await _wardService.GetWardByIdAsync(id);
                if (ward == null)
                {
                    return NotFound($"Ward with ID {id} not found");
                }

                return Ok(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ward with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving ward with ID {id}");
            }
        }

        // GET: api/Ward/code/{code}
        [HttpGet("code/{code}")]
        public async Task<ActionResult<WardDTO>> GetWardByCode(string code)
        {
            try
            {
                var ward = await _wardService.GetWardByCodeAsync(code);
                if (ward == null)
                {
                    return NotFound($"Ward with code {code} not found");
                }

                return Ok(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ward with code {Code}", code);
                return StatusCode(500, $"An error occurred while retrieving ward with code {code}");
            }
        }

        // GET: api/Ward/district/{districtId}
        [HttpGet("district/{districtId}")]
        public async Task<ActionResult<IEnumerable<WardDTO>>> GetWardsByDistrict(long districtId)
        {
            try
            {
                var wards = await _wardService.GetWardsByDistrictIdAsync(districtId);
                return Ok(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wards for district ID {DistrictId}", districtId);
                return StatusCode(500, $"An error occurred while retrieving wards for district ID {districtId}");
            }
        }

        // GET: api/Ward/province/{provinceId}
        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<IEnumerable<WardDTO>>> GetWardsByProvince(long provinceId)
        {
            try
            {
                var wards = await _wardService.GetWardsByProvinceIdAsync(provinceId);
                return Ok(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wards for province ID {ProvinceId}", provinceId);
                return StatusCode(500, $"An error occurred while retrieving wards for province ID {provinceId}");
            }
        }

        // GET: api/Ward/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountWards([FromQuery] long? districtId, [FromQuery] long? provinceId)
        {
            try
            {
                var count = await _wardService.CountWardsAsync(districtId, provinceId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting wards");
                return StatusCode(500, "An error occurred while counting wards");
            }
        }

        // POST: api/Ward
        [HttpPost]
        public async Task<ActionResult<WardDTO>> CreateWard(WardCreateDTO wardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var ward = await _wardService.CreateWardAsync(wardDto);
                return CreatedAtAction(nameof(GetWard), new { id = ward.Id }, ward);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating ward");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ward");
                return StatusCode(500, "An error occurred while creating ward");
            }
        }

        // PUT: api/Ward/5
        [HttpPut("{id}")]
        public async Task<ActionResult<WardDTO>> UpdateWard(long id, WardUpdateDTO wardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var ward = await _wardService.UpdateWardAsync(id, wardDto);
                if (ward == null)
                {
                    return NotFound($"Ward with ID {id} not found");
                }

                return Ok(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ward with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating ward with ID {id}");
            }
        }

        // DELETE: api/Ward/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWard(long id)
        {
            try
            {
                var result = await _wardService.DeleteWardAsync(id);
                if (!result)
                {
                    return NotFound($"Ward with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ward with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting ward with ID {id}");
            }
        }
    }
}
