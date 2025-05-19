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
    public class ApiUsageTrackingService : IApiUsageTrackingService
    {
        private readonly IRepository<ApiUsageTracking> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApiUsageTrackingService> _logger;

        public ApiUsageTrackingService(IRepository<ApiUsageTracking> repository, IMapper mapper, ILogger<ApiUsageTrackingService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ApiUsageTrackingDTO>> GetAllApiUsageTrackingsAsync(int? skip = null, int? take = null)
        {
            try
            {
                var trackings = await _repository.GetAsync(
                    orderBy: q => q.OrderByDescending(t => t.CalledAt),
                    includeProperties: "ApiKey,ApiKey.User",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<ApiUsageTrackingDTO>>(trackings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all API usage trackings");
                throw;
            }
        }

        public async Task<IEnumerable<ApiUsageTrackingDTO>> GetApiUsageTrackingsByApiKeyIdAsync(long apiKeyId)
        {
            try
            {
                var trackings = await _repository.GetAsync(
                    filter: t => t.ApiKeyId == apiKeyId,
                    orderBy: q => q.OrderByDescending(t => t.CalledAt),
                    includeProperties: "ApiKey,ApiKey.User");

                return _mapper.Map<IEnumerable<ApiUsageTrackingDTO>>(trackings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting API usage trackings for API key ID {ApiKeyId}", apiKeyId);
                throw;
            }
        }

        public async Task<ApiUsageTrackingDTO> GetApiUsageTrackingByIdAsync(long id)
        {
            try
            {
                var tracking = await _repository.GetFirstOrDefaultAsync(
                    filter: t => t.Id == id,
                    includeProperties: "ApiKey,ApiKey.User");

                if (tracking == null)
                {
                    _logger.LogWarning("API usage tracking with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<ApiUsageTrackingDTO>(tracking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting API usage tracking with ID {Id}", id);
                throw;
            }
        }

        public async Task<ApiUsageTrackingDTO> CreateApiUsageTrackingAsync(ApiUsageTrackingCreateDTO trackingDto)
        {
            try
            {
                var tracking = _mapper.Map<ApiUsageTracking>(trackingDto);
                await _repository.AddAsync(tracking);

                // Reload the tracking with API key and user details
                tracking = await _repository.GetFirstOrDefaultAsync(
                    filter: t => t.Id == tracking.Id,
                    includeProperties: "ApiKey,ApiKey.User");

                return _mapper.Map<ApiUsageTrackingDTO>(tracking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating API usage tracking for API key ID {ApiKeyId}", trackingDto.ApiKeyId);
                throw;
            }
        }

        public async Task<int> CountApiUsageTrackingsAsync(long? apiKeyId = null)
        {
            try
            {
                if (apiKeyId.HasValue)
                {
                    return await _repository.CountAsync(t => t.ApiKeyId == apiKeyId.Value);
                }
                else
                {
                    return await _repository.CountAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting API usage trackings");
                throw;
            }
        }
    }
}
