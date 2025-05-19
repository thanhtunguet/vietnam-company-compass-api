using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IDistrictService
    {
        Task<IEnumerable<DistrictDTO>> GetAllDistrictsAsync(int? skip = null, int? take = null);
        Task<IEnumerable<DistrictDTO>> GetDistrictsByProvinceIdAsync(long provinceId);
        Task<DistrictDTO> GetDistrictByIdAsync(long id);
        Task<DistrictDTO> GetDistrictByCodeAsync(string code);
        Task<DistrictDTO> CreateDistrictAsync(DistrictCreateDTO districtDto);
        Task<DistrictDTO> UpdateDistrictAsync(long id, DistrictUpdateDTO districtDto);
        Task<bool> DeleteDistrictAsync(long id);
        Task<int> CountDistrictsAsync(long? provinceId = null);
    }
}
