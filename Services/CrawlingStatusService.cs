using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;
using VietnamBusiness.Models;
using VietnamBusiness.Repositories;

namespace VietnamBusiness.Services
{
    public class CrawlingStatusService : ICrawlingStatusService
    {
        private readonly IRepository<CrawlingStatus> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CrawlingStatusService> _logger;

        public CrawlingStatusService(IRepository<CrawlingStatus> repository, IMapper mapper, ILogger<CrawlingStatusService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CrawlingStatusDTO>> GetAllCrawlingStatusesAsync()
        {
            try
            {
                var statuses = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<CrawlingStatusDTO>>(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all crawling statuses");
                throw;
            }
        }

        public async Task<CrawlingStatusDTO> GetCrawlingStatusByIdAsync(long id)
        {
            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("CrawlingStatus with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<CrawlingStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting crawling status with ID {Id}", id);
                throw;
            }
        }

        public async Task<CrawlingStatusDTO> GetCrawlingStatusByCodeAsync(string code)
        {
            try
            {
                var status = await _repository.GetFirstOrDefaultAsync(s => s.Code == code);
                if (status == null)
                {
                    _logger.LogWarning("CrawlingStatus with Code {Code} not found", code);
                    return null;
                }

                return _mapper.Map<CrawlingStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting crawling status with Code {Code}", code);
                throw;
            }
        }

        public async Task<CrawlingStatusDTO> CreateCrawlingStatusAsync(CrawlingStatusCreateDTO crawlingStatusDto)
        {
            try
            {
                var existingStatus = await _repository.GetFirstOrDefaultAsync(s => s.Code == crawlingStatusDto.Code);
                if (existingStatus != null)
                {
                    _logger.LogWarning("CrawlingStatus with Code {Code} already exists", crawlingStatusDto.Code);
                    throw new InvalidOperationException($"Crawling status with code {crawlingStatusDto.Code} already exists");
                }

                var status = _mapper.Map<CrawlingStatus>(crawlingStatusDto);
                await _repository.AddAsync(status);

                return _mapper.Map<CrawlingStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating crawling status with Code {Code}", crawlingStatusDto.Code);
                throw;
            }
        }

        public async Task<CrawlingStatusDTO> UpdateCrawlingStatusAsync(long id, CrawlingStatusUpdateDTO crawlingStatusDto)
        {
            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("CrawlingStatus with ID {Id} not found for update", id);
                    return null;
                }

                _mapper.Map(crawlingStatusDto, status);
                await _repository.UpdateAsync(status);

                return _mapper.Map<CrawlingStatusDTO>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating crawling status with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCrawlingStatusAsync(long id)
        {
            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("CrawlingStatus with ID {Id} not found for deletion", id);
                    return false;
                }

                await _repository.DeleteAsync(status);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting crawling status with ID {Id}", id);
                throw;
            }
        }
    }
}
