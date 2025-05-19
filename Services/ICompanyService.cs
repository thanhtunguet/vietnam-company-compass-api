using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(int? skip = null, int? take = null);
        Task<IEnumerable<CompanyDTO>> GetCompaniesByProvinceIdAsync(long provinceId, int? skip = null, int? take = null);
        Task<IEnumerable<CompanyDTO>> GetCompaniesByDistrictIdAsync(long districtId, int? skip = null, int? take = null);
        Task<IEnumerable<CompanyDTO>> GetCompaniesByWardIdAsync(long wardId, int? skip = null, int? take = null);
        Task<IEnumerable<CompanyDTO>> GetCompaniesByStatusIdAsync(long statusId, int? skip = null, int? take = null);
        Task<CompanyDTO> GetCompanyByIdAsync(long id);
        Task<CompanyDTO> GetCompanyByTaxCodeAsync(string taxCode);
        Task<CompanyDTO> CreateCompanyAsync(CompanyCreateDTO companyDto);
        Task<CompanyDTO> UpdateCompanyAsync(long id, CompanyUpdateDTO companyDto);
        Task<bool> DeleteCompanyAsync(long id);
        Task<int> CountCompaniesAsync(long? provinceId = null, long? districtId = null, long? wardId = null, long? statusId = null);
    }
}
