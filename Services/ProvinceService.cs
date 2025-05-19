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
    public class ProvinceService : IProvinceService
    {
        private readonly IRepository<Province> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProvinceService> _logger;

        public ProvinceService(IRepository<Province> repository, IMapper mapper, ILogger<ProvinceService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ProvinceDTO>> GetAllProvincesAsync(int? skip = null, int? take = null)
        {
            try
            {
                var provinces = await _repository.GetAsync(
                    filter: p => p.DeletedAt == null,
                    orderBy: q => q.OrderBy(p => p.Name),
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<ProvinceDTO>>(provinces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all provinces");
                throw;
            }
        }

        public async Task<ProvinceDTO> GetProvinceByIdAsync(long id)
        {
            try
            {
                var province = await _repository.GetByIdAsync(id);
                if (province == null || province.DeletedAt != null)
                {
                    _logger.LogWarning("Province with ID {Id} not found or deleted", id);
                    return null;
                }

                return _mapper.Map<ProvinceDTO>(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting province with ID {Id}", id);
                throw;
            }
        }

        public async Task<ProvinceDTO> GetProvinceByCodeAsync(string code)
        {
            try
            {
                var province = await _repository.GetFirstOrDefaultAsync(
                    p => p.Code == code && p.DeletedAt == null);
                
                if (province == null)
                {
                    _logger.LogWarning("Province with Code {Code} not found or deleted", code);
                    return null;
                }

                return _mapper.Map<ProvinceDTO>(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting province with Code {Code}", code);
                throw;
            }
        }

        public async Task<ProvinceDTO> CreateProvinceAsync(ProvinceCreateDTO provinceDto)
        {
            try
            {
                var existingProvince = await _repository.GetFirstOrDefaultAsync(p => p.Code == provinceDto.Code);
                if (existingProvince != null)
                {
                    _logger.LogWarning("Province with Code {Code} already exists", provinceDto.Code);
                    throw new InvalidOperationException($"Province with code {provinceDto.Code} already exists");
                }

                var province = _mapper.Map<Province>(provinceDto);
                await _repository.AddAsync(province);

                return _mapper.Map<ProvinceDTO>(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating province with Code {Code}", provinceDto.Code);
                throw;
            }
        }

        public async Task<ProvinceDTO> UpdateProvinceAsync(long id, ProvinceUpdateDTO provinceDto)
        {
            try
            {
                var province = await _repository.GetByIdAsync(id);
                if (province == null || province.DeletedAt != null)
                {
                    _logger.LogWarning("Province with ID {Id} not found or deleted for update", id);
                    return null;
                }

                _mapper.Map(provinceDto, province);
                await _repository.UpdateAsync(province);

                return _mapper.Map<ProvinceDTO>(province);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating province with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteProvinceAsync(long id)
        {
            try
            {
                var province = await _repository.GetByIdAsync(id);
                if (province == null || province.DeletedAt != null)
                {
                    _logger.LogWarning("Province with ID {Id} not found or already deleted", id);
                    return false;
                }

                // Soft delete
                province.DeletedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(province);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting province with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountProvincesAsync()
        {
            try
            {
                return await _repository.CountAsync(p => p.DeletedAt == null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting provinces");
                throw;
            }
        }
    }
}
