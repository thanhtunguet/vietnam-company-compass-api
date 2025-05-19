using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;
using VietnamBusiness.Models;
using VietnamBusiness.Repositories;

namespace VietnamBusiness.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IRepository<ApiKey> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApiKeyService> _logger;

        public ApiKeyService(IRepository<ApiKey> repository, IMapper mapper, ILogger<ApiKeyService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ApiKeyDTO>> GetAllApiKeysAsync(int? skip = null, int? take = null)
        {
            try
            {
                var apiKeys = await _repository.GetAsync(
                    orderBy: q => q.OrderByDescending(k => k.CreatedAt),
                    includeProperties: "User",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<ApiKeyDTO>>(apiKeys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all API keys");
                throw;
            }
        }

        public async Task<IEnumerable<ApiKeyDTO>> GetApiKeysByUserIdAsync(long userId)
        {
            try
            {
                var apiKeys = await _repository.GetAsync(
                    filter: k => k.UserId == userId,
                    orderBy: q => q.OrderByDescending(k => k.CreatedAt),
                    includeProperties: "User");

                return _mapper.Map<IEnumerable<ApiKeyDTO>>(apiKeys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting API keys for user ID {UserId}", userId);
                throw;
            }
        }

        public async Task<ApiKeyDTO> GetApiKeyByIdAsync(long id)
        {
            try
            {
                var apiKey = await _repository.GetFirstOrDefaultAsync(
                    filter: k => k.Id == id,
                    includeProperties: "User");

                if (apiKey == null)
                {
                    _logger.LogWarning("API key with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<ApiKeyDTO>(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting API key with ID {Id}", id);
                throw;
            }
        }

        public async Task<ApiKeyDTO> GetApiKeyByKeyAsync(string key)
        {
            try
            {
                var apiKey = await _repository.GetFirstOrDefaultAsync(
                    filter: k => k.Key == key,
                    includeProperties: "User");

                if (apiKey == null)
                {
                    _logger.LogWarning("API key {Key} not found", key);
                    return null;
                }

                return _mapper.Map<ApiKeyDTO>(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting API key {Key}", key);
                throw;
            }
        }

        public async Task<ApiKeyDTO> CreateApiKeyAsync(ApiKeyCreateDTO apiKeyDto)
        {
            try
            {
                var apiKey = _mapper.Map<ApiKey>(apiKeyDto);
                apiKey.Key = GenerateApiKey();
                
                await _repository.AddAsync(apiKey);

                // Reload the ApiKey with user details
                apiKey = await _repository.GetFirstOrDefaultAsync(
                    filter: k => k.Id == apiKey.Id,
                    includeProperties: "User");

                return _mapper.Map<ApiKeyDTO>(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating API key for user ID {UserId}", apiKeyDto.UserId);
                throw;
            }
        }

        public async Task<ApiKeyDTO> UpdateApiKeyAsync(long id, ApiKeyUpdateDTO apiKeyDto)
        {
            try
            {
                var apiKey = await _repository.GetByIdAsync(id);
                if (apiKey == null)
                {
                    _logger.LogWarning("API key with ID {Id} not found for update", id);
                    return null;
                }

                _mapper.Map(apiKeyDto, apiKey);
                await _repository.UpdateAsync(apiKey);

                // Reload the ApiKey with user details
                apiKey = await _repository.GetFirstOrDefaultAsync(
                    filter: k => k.Id == id,
                    includeProperties: "User");

                return _mapper.Map<ApiKeyDTO>(apiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating API key with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteApiKeyAsync(long id)
        {
            try
            {
                var apiKey = await _repository.GetByIdAsync(id);
                if (apiKey == null)
                {
                    _logger.LogWarning("API key with ID {Id} not found for deletion", id);
                    return false;
                }

                // Deactivate API key instead of deleting
                apiKey.IsActive = false;
                await _repository.UpdateAsync(apiKey);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting API key with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> IncrementApiKeyUsageAsync(long id)
        {
            try
            {
                var apiKey = await _repository.GetByIdAsync(id);
                if (apiKey == null)
                {
                    _logger.LogWarning("API key with ID {Id} not found for incrementing usage", id);
                    return false;
                }

                apiKey.RequestsUsed++;
                await _repository.UpdateAsync(apiKey);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing usage for API key with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ValidateApiKeyAsync(string key)
        {
            try
            {
                var apiKey = await _repository.GetFirstOrDefaultAsync(
                    filter: k => k.Key == key && k.IsActive);

                if (apiKey == null)
                {
                    _logger.LogWarning("Invalid or inactive API key: {Key}", key);
                    return false;
                }

                // Check if usage limit has been reached
                if (apiKey.RequestsUsed >= apiKey.RequestLimit)
                {
                    _logger.LogWarning("API key {Key} has reached its usage limit", key);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating API key {Key}", key);
                throw;
            }
        }

        private string GenerateApiKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-").Substring(0, 64);
            }
        }
    }
}
