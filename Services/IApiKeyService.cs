using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IApiKeyService
    {
        Task<IEnumerable<ApiKeyDTO>> GetAllApiKeysAsync(int? skip = null, int? take = null);
        Task<IEnumerable<ApiKeyDTO>> GetApiKeysByUserIdAsync(long userId);
        Task<ApiKeyDTO> GetApiKeyByIdAsync(long id);
        Task<ApiKeyDTO> GetApiKeyByKeyAsync(string key);
        Task<ApiKeyDTO> CreateApiKeyAsync(ApiKeyCreateDTO apiKeyDto);
        Task<ApiKeyDTO> UpdateApiKeyAsync(long id, ApiKeyUpdateDTO apiKeyDto);
        Task<bool> DeleteApiKeyAsync(long id);
        Task<bool> IncrementApiKeyUsageAsync(long id);
        Task<bool> ValidateApiKeyAsync(string key);
    }
}
