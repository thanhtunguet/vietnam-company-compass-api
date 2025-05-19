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
    public class WardService : IWardService
    {
        private readonly IRepository<Ward> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<WardService> _logger;

        public WardService(IRepository<Ward> repository, IMapper mapper, ILogger<WardService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<WardDTO>> GetAllWardsAsync(int? skip = null, int? take = null)
        {
            try
            {
                var wards = await _repository.GetAsync(
                    filter: w => w.DeletedAt == null,
                    orderBy: q => q.OrderBy(w => w.Name),
                    includeProperties: "Province,District",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<WardDTO>>(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all wards");
                throw;
            }
        }

        public async Task<IEnumerable<WardDTO>> GetWardsByDistrictIdAsync(long districtId)
        {
            try
            {
                var wards = await _repository.GetAsync(
                    filter: w => w.DistrictId == districtId && w.DeletedAt == null,
                    orderBy: q => q.OrderBy(w => w.Name),
                    includeProperties: "Province,District");

                return _mapper.Map<IEnumerable<WardDTO>>(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wards for district ID {DistrictId}", districtId);
                throw;
            }
        }

        public async Task<IEnumerable<WardDTO>> GetWardsByProvinceIdAsync(long provinceId)
        {
            try
            {
                var wards = await _repository.GetAsync(
                    filter: w => w.ProvinceId == provinceId && w.DeletedAt == null,
                    orderBy: q => q.OrderBy(w => w.Name),
                    includeProperties: "Province,District");

                return _mapper.Map<IEnumerable<WardDTO>>(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wards for province ID {ProvinceId}", provinceId);
                throw;
            }
        }

        public async Task<WardDTO> GetWardByIdAsync(long id)
        {
            try
            {
                var ward = await _repository.GetFirstOrDefaultAsync(
                    filter: w => w.Id == id && w.DeletedAt == null,
                    includeProperties: "Province,District");

                if (ward == null)
                {
                    _logger.LogWarning("Ward with ID {Id} not found or deleted", id);
                    return null;
                }

                return _mapper.Map<WardDTO>(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ward with ID {Id}", id);
                throw;
            }
        }

        public async Task<WardDTO> GetWardByCodeAsync(string code)
        {
            try
            {
                var ward = await _repository.GetFirstOrDefaultAsync(
                    filter: w => w.Code == code && w.DeletedAt == null,
                    includeProperties: "Province,District");

                if (ward == null)
                {
                    _logger.LogWarning("Ward with Code {Code} not found or deleted", code);
                    return null;
                }

                return _mapper.Map<WardDTO>(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ward with Code {Code}", code);
                throw;
            }
        }

        public async Task<WardDTO> CreateWardAsync(WardCreateDTO wardDto)
        {
            try
            {
                var existingWard = await _repository.GetFirstOrDefaultAsync(w => w.Code == wardDto.Code);
                if (existingWard != null)
                {
                    _logger.LogWarning("Ward with Code {Code} already exists", wardDto.Code);
                    throw new InvalidOperationException($"Ward with code {wardDto.Code} already exists");
                }

                var ward = _mapper.Map<Ward>(wardDto);
                await _repository.AddAsync(ward);

                // Reload the ward with province and district details
                ward = await _repository.GetFirstOrDefaultAsync(
                    filter: w => w.Id == ward.Id,
                    includeProperties: "Province,District");

                return _mapper.Map<WardDTO>(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ward with Code {Code}", wardDto.Code);
                throw;
            }
        }

        public async Task<WardDTO> UpdateWardAsync(long id, WardUpdateDTO wardDto)
        {
            try
            {
                var ward = await _repository.GetFirstOrDefaultAsync(
                    filter: w => w.Id == id && w.DeletedAt == null);

                if (ward == null)
                {
                    _logger.LogWarning("Ward with ID {Id} not found or deleted for update", id);
                    return null;
                }

                _mapper.Map(wardDto, ward);
                await _repository.UpdateAsync(ward);

                // Reload the ward with province and district details
                ward = await _repository.GetFirstOrDefaultAsync(
                    filter: w => w.Id == id,
                    includeProperties: "Province,District");

                return _mapper.Map<WardDTO>(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ward with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteWardAsync(long id)
        {
            try
            {
                var ward = await _repository.GetByIdAsync(id);
                if (ward == null || ward.DeletedAt != null)
                {
                    _logger.LogWarning("Ward with ID {Id} not found or already deleted", id);
                    return false;
                }

                // Soft delete
                ward.DeletedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(ward);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ward with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountWardsAsync(long? districtId = null, long? provinceId = null)
        {
            try
            {
                if (districtId.HasValue)
                {
                    return await _repository.CountAsync(
                        w => w.DistrictId == districtId.Value && w.DeletedAt == null);
                }
                else if (provinceId.HasValue)
                {
                    return await _repository.CountAsync(
                        w => w.ProvinceId == provinceId.Value && w.DeletedAt == null);
                }
                else
                {
                    return await _repository.CountAsync(w => w.DeletedAt == null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting wards");
                throw;
            }
        }
    }
}
