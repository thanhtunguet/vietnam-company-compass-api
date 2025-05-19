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
    public class CompanyBusinessMappingService : ICompanyBusinessMappingService
    {
        private readonly IRepository<CompanyBusinessMapping> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyBusinessMappingService> _logger;

        public CompanyBusinessMappingService(IRepository<CompanyBusinessMapping> repository, IMapper mapper, ILogger<CompanyBusinessMappingService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CompanyBusinessMappingDTO>> GetAllMappingsAsync()
        {
            try
            {
                var mappings = await _repository.GetAsync(
                    includeProperties: "Company,Business");

                return _mapper.Map<IEnumerable<CompanyBusinessMappingDTO>>(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all company-business mappings");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyBusinessMappingDTO>> GetMappingsByCompanyIdAsync(long companyId)
        {
            try
            {
                var mappings = await _repository.GetAsync(
                    filter: m => m.CompanyId == companyId,
                    includeProperties: "Company,Business");

                return _mapper.Map<IEnumerable<CompanyBusinessMappingDTO>>(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mappings for company ID {CompanyId}", companyId);
                throw;
            }
        }

        public async Task<IEnumerable<CompanyBusinessMappingDTO>> GetMappingsByBusinessIdAsync(long businessId)
        {
            try
            {
                var mappings = await _repository.GetAsync(
                    filter: m => m.BusinessId == businessId,
                    includeProperties: "Company,Business");

                return _mapper.Map<IEnumerable<CompanyBusinessMappingDTO>>(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mappings for business ID {BusinessId}", businessId);
                throw;
            }
        }

        public async Task<CompanyBusinessMappingDTO> GetMappingAsync(long companyId, long businessId)
        {
            try
            {
                var mapping = await _repository.GetFirstOrDefaultAsync(
                    filter: m => m.CompanyId == companyId && m.BusinessId == businessId,
                    includeProperties: "Company,Business");

                if (mapping == null)
                {
                    _logger.LogWarning("Mapping between company ID {CompanyId} and business ID {BusinessId} not found", companyId, businessId);
                    return null;
                }

                return _mapper.Map<CompanyBusinessMappingDTO>(mapping);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mapping between company ID {CompanyId} and business ID {BusinessId}", companyId, businessId);
                throw;
            }
        }

        public async Task<CompanyBusinessMappingDTO> CreateMappingAsync(CompanyBusinessMappingCreateDTO mappingDto)
        {
            try
            {
                var existingMapping = await _repository.GetFirstOrDefaultAsync(
                    m => m.CompanyId == mappingDto.CompanyId && m.BusinessId == mappingDto.BusinessId);

                if (existingMapping != null)
                {
                    _logger.LogWarning("Mapping between company ID {CompanyId} and business ID {BusinessId} already exists", 
                        mappingDto.CompanyId, mappingDto.BusinessId);
                    
                    throw new InvalidOperationException($"Mapping between company ID {mappingDto.CompanyId} and business ID {mappingDto.BusinessId} already exists");
                }

                var mapping = _mapper.Map<CompanyBusinessMapping>(mappingDto);
                await _repository.AddAsync(mapping);

                // Reload the mapping with company and business details
                mapping = await _repository.GetFirstOrDefaultAsync(
                    filter: m => m.CompanyId == mapping.CompanyId && m.BusinessId == mapping.BusinessId,
                    includeProperties: "Company,Business");

                return _mapper.Map<CompanyBusinessMappingDTO>(mapping);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating mapping between company ID {CompanyId} and business ID {BusinessId}", 
                    mappingDto.CompanyId, mappingDto.BusinessId);
                throw;
            }
        }

        public async Task<bool> DeleteMappingAsync(long companyId, long businessId)
        {
            try
            {
                var mapping = await _repository.GetFirstOrDefaultAsync(
                    m => m.CompanyId == companyId && m.BusinessId == businessId);

                if (mapping == null)
                {
                    _logger.LogWarning("Mapping between company ID {CompanyId} and business ID {BusinessId} not found for deletion", 
                        companyId, businessId);
                    return false;
                }

                await _repository.DeleteAsync(mapping);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting mapping between company ID {CompanyId} and business ID {BusinessId}", 
                    companyId, businessId);
                throw;
            }
        }
    }
}
