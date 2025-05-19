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
    public class CompanyStatusService : ICompanyStatusService
    {
        private readonly IRepository<CompanyStatus> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyStatusService> _logger;

        public CompanyStatusService(IRepository<CompanyStatus> repository, IMapper mapper, ILogger<CompanyStatusService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CompanyStatusDTO>> GetAllCompanyStatusesAsync()
        {
            try
            {
                var statuses = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyStatusDTO>>(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all company statuses");
                throw;
            }
        }

        public async Task<CompanyStatusDTO> GetCompanyStatusByIdAsync(long id)
        {
            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("CompanyStatus with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<CompanyStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company status with ID {Id}", id);
                throw;
            }
        }

        public async Task<CompanyStatusDTO> GetCompanyStatusByCodeAsync(string code)
        {
            try
            {
                var status = await _repository.GetFirstOrDefaultAsync(s => s.Code == code);
                if (status == null)
                {
                    _logger.LogWarning("CompanyStatus with Code {Code} not found", code);
                    return null;
                }

                return _mapper.Map<CompanyStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company status with Code {Code}", code);
                throw;
            }
        }

        public async Task<CompanyStatusDTO> CreateCompanyStatusAsync(CompanyStatusCreateDTO companyStatusDto)
        {
            try
            {
                // Check if status with the same code already exists
                var existingStatus = await _repository.GetFirstOrDefaultAsync(s => s.Code == companyStatusDto.Code);
                if (existingStatus != null)
                {
                    _logger.LogWarning("CompanyStatus with Code {Code} already exists", companyStatusDto.Code);
                    throw new InvalidOperationException($"Company status with code {companyStatusDto.Code} already exists");
                }

                var status = _mapper.Map<CompanyStatus>(companyStatusDto);
                await _repository.AddAsync(status);

                return _mapper.Map<CompanyStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company status with Code {Code}", companyStatusDto.Code);
                throw;
            }
        }

        public async Task<CompanyStatusDTO> UpdateCompanyStatusAsync(long id, CompanyStatusUpdateDTO companyStatusDto)
        {
            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("CompanyStatus with ID {Id} not found for update", id);
                    return null;
                }

                _mapper.Map(companyStatusDto, status);
                await _repository.UpdateAsync(status);

                return _mapper.Map<CompanyStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company status with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCompanyStatusAsync(long id)
        {
            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("CompanyStatus with ID {Id} not found for deletion", id);
                    return false;
                }

                await _repository.DeleteAsync(status);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting company status with ID {Id}", id);
                throw;
            }
        }
    }
}
