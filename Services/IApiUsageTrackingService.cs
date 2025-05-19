using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IApiUsageTrackingService
    {
        Task<IEnumerable<ApiUsageTrackingDTO>> GetAllApiUsageTrackingsAsync(int? skip = null, int? take = null);
        Task<IEnumerable<ApiUsageTrackingDTO>> GetApiUsageTrackingsByApiKeyIdAsync(long apiKeyId);
        Task<ApiUsageTrackingDTO> GetApiUsageTrackingByIdAsync(long id);
        Task<ApiUsageTrackingDTO> CreateApiUsageTrackingAsync(ApiUsageTrackingCreateDTO trackingDto);
        Task<int> CountApiUsageTrackingsAsync(long? apiKeyId = null);
    }
}
