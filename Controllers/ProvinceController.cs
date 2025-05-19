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
    [Route("api/provinces")]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly ILogger<ProvinceController> _logger;

        public ProvinceController(IProvinceService provinceService, ILogger<ProvinceController> logger)
        {
            _provinceService = provinceService ?? throw new ArgumentNullException(nameof(provinceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Province
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProvinceDTO>>> GetProvinces([FromQuery] int? skip, [FromQuery] int? take)
        {
            try
            {
                var provinces = await _provinceService.GetAllProvincesAsync(skip, take);
                return Ok(provinces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving provinces");
                return StatusCode(500, "An error occurred while retrieving provinces");
            }
        }

        // GET: api/Province/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProvinceDTO>> GetProvince(long id)
        {
            try
            {
                var province = await _provinceService.GetProvinceByIdAsync(id);
                if (province == null)
                {
                    return NotFound($"Province with ID {id} not found");
                }

                return Ok(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving province with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving province with ID {id}");
            }
        }

        // GET: api/Province/code/{code}
        [HttpGet("code/{code}")]
        public async Task<ActionResult<ProvinceDTO>> GetProvinceByCode(string code)
        {
            try
            {
                var province = await _provinceService.GetProvinceByCodeAsync(code);
                if (province == null)
                {
                    return NotFound($"Province with code {code} not found");
                }

                return Ok(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving province with code {Code}", code);
                return StatusCode(500, $"An error occurred while retrieving province with code {code}");
            }
        }

        // GET: api/Province/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> CountProvinces()
        {
            try
            {
                var count = await _provinceService.CountProvincesAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting provinces");
                return StatusCode(500, "An error occurred while counting provinces");
            }
        }

        // POST: api/Province
        [HttpPost]
        public async Task<ActionResult<ProvinceDTO>> CreateProvince(ProvinceCreateDTO provinceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var province = await _provinceService.CreateProvinceAsync(provinceDto);
                return CreatedAtAction(nameof(GetProvince), new { id = province.Id }, province);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating province");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating province");
                return StatusCode(500, "An error occurred while creating province");
            }
        }

        // PUT: api/Province/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProvinceDTO>> UpdateProvince(long id, ProvinceUpdateDTO provinceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var province = await _provinceService.UpdateProvinceAsync(id, provinceDto);
                if (province == null)
                {
                    return NotFound($"Province with ID {id} not found");
                }

                return Ok(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating province with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating province with ID {id}");
            }
        }

        // DELETE: api/Province/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProvince(long id)
        {
            try
            {
                var result = await _provinceService.DeleteProvinceAsync(id);
                if (!result)
                {
                    return NotFound($"Province with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting province with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting province with ID {id}");
            }
        }
    }
}
