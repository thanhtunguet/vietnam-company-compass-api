using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface ICompanyStatusService
    {
        Task<IEnumerable<CompanyStatusDTO>> GetAllCompanyStatusesAsync();
        Task<CompanyStatusDTO> GetCompanyStatusByIdAsync(long id);
        Task<CompanyStatusDTO> GetCompanyStatusByCodeAsync(string code);
        Task<CompanyStatusDTO> CreateCompanyStatusAsync(CompanyStatusCreateDTO companyStatusDto);
        Task<CompanyStatusDTO> UpdateCompanyStatusAsync(long id, CompanyStatusUpdateDTO companyStatusDto);
        Task<bool> DeleteCompanyStatusAsync(long id);
    }
}
