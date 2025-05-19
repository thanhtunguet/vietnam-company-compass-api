using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;
using VietnamBusiness.Models;
using VietnamBusiness.Repositories;

namespace VietnamBusiness.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IRepository<District> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DistrictService> _logger;

        public DistrictService(IRepository<District> repository, IMapper mapper, ILogger<DistrictService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DistrictDTO>> GetAllDistrictsAsync(int? skip = null, int? take = null)
        {
            try
            {
                var districts = await _repository.GetAsync(
                    filter: d => d.DeletedAt == null,
                    orderBy: q => q.OrderBy(d => d.Name),
                    includeProperties: "Province",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<DistrictDTO>>(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all districts");
                throw;
            }
        }

        public async Task<IEnumerable<DistrictDTO>> GetDistrictsByProvinceIdAsync(long provinceId)
        {
            try
            {
                var districts = await _repository.GetAsync(
                    filter: d => d.ProvinceId == provinceId && d.DeletedAt == null,
                    orderBy: q => q.OrderBy(d => d.Name),
                    includeProperties: "Province");

                return _mapper.Map<IEnumerable<DistrictDTO>>(districts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting districts for province ID {ProvinceId}", provinceId);
                throw;
            }
        }

        public async Task<DistrictDTO> GetDistrictByIdAsync(long id)
        {
            try
            {
                var district = await _repository.GetFirstOrDefaultAsync(
                    filter: d => d.Id == id && d.DeletedAt == null,
                    includeProperties: "Province");

                if (district == null)
                {
                    _logger.LogWarning("District with ID {Id} not found or deleted", id);
                    return null;
                }

                return _mapper.Map<DistrictDTO>(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting district with ID {Id}", id);
                throw;
            }
        }

        public async Task<DistrictDTO> GetDistrictByCodeAsync(string code)
        {
            try
            {
                var district = await _repository.GetFirstOrDefaultAsync(
                    filter: d => d.Code == code && d.DeletedAt == null,
                    includeProperties: "Province");

                if (district == null)
                {
                    _logger.LogWarning("District with Code {Code} not found or deleted", code);
                    return null;
                }

                return _mapper.Map<DistrictDTO>(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting district with Code {Code}", code);
                throw;
            }
        }

        public async Task<DistrictDTO> CreateDistrictAsync(DistrictCreateDTO districtDto)
        {
            try
            {
                var existingDistrict = await _repository.GetFirstOrDefaultAsync(d => d.Code == districtDto.Code);
                if (existingDistrict != null)
                {
                    _logger.LogWarning("District with Code {Code} already exists", districtDto.Code);
                    throw new InvalidOperationException($"District with code {districtDto.Code} already exists");
                }

                var district = _mapper.Map<District>(districtDto);
                await _repository.AddAsync(district);

                // Reload the district with province details
                district = await _repository.GetFirstOrDefaultAsync(
                    filter: d => d.Id == district.Id,
                    includeProperties: "Province");

                return _mapper.Map<DistrictDTO>(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating district with Code {Code}", districtDto.Code);
                throw;
            }
        }

        public async Task<DistrictDTO> UpdateDistrictAsync(long id, DistrictUpdateDTO districtDto)
        {
            try
            {
                var district = await _repository.GetFirstOrDefaultAsync(
                    filter: d => d.Id == id && d.DeletedAt == null);

                if (district == null)
                {
                    _logger.LogWarning("District with ID {Id} not found or deleted for update", id);
                    return null;
                }

                _mapper.Map(districtDto, district);
                await _repository.UpdateAsync(district);

                // Reload the district with province details
                district = await _repository.GetFirstOrDefaultAsync(
                    filter: d => d.Id == id,
                    includeProperties: "Province");

                return _mapper.Map<DistrictDTO>(district);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating district with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteDistrictAsync(long id)
        {
            try
            {
                var district = await _repository.GetByIdAsync(id);
                if (district == null || district.DeletedAt != null)
                {
                    _logger.LogWarning("District with ID {Id} not found or already deleted", id);
                    return false;
                }

                // Soft delete
                district.DeletedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(district);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting district with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountDistrictsAsync(long? provinceId = null)
        {
            try
            {
                if (provinceId.HasValue)
                {
                    return await _repository.CountAsync(
                        d => d.ProvinceId == provinceId.Value && d.DeletedAt == null);
                }
                else
                {
                    return await _repository.CountAsync(d => d.DeletedAt == null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting districts");
                throw;
            }
        }
    }
}
