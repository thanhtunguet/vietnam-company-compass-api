using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface ICompanyBusinessMappingService
    {
        Task<IEnumerable<CompanyBusinessMappingDTO>> GetAllMappingsAsync();
        Task<IEnumerable<CompanyBusinessMappingDTO>> GetMappingsByCompanyIdAsync(long companyId);
        Task<IEnumerable<CompanyBusinessMappingDTO>> GetMappingsByBusinessIdAsync(long businessId);
        Task<CompanyBusinessMappingDTO> GetMappingAsync(long companyId, long businessId);
        Task<CompanyBusinessMappingDTO> CreateMappingAsync(CompanyBusinessMappingCreateDTO mappingDto);
        Task<bool> DeleteMappingAsync(long companyId, long businessId);
    }
}
