using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IWardService
    {
        Task<IEnumerable<WardDTO>> GetAllWardsAsync(int? skip = null, int? take = null);
        Task<IEnumerable<WardDTO>> GetWardsByDistrictIdAsync(long districtId);
        Task<IEnumerable<WardDTO>> GetWardsByProvinceIdAsync(long provinceId);
        Task<WardDTO> GetWardByIdAsync(long id);
        Task<WardDTO> GetWardByCodeAsync(string code);
        Task<WardDTO> CreateWardAsync(WardCreateDTO wardDto);
        Task<WardDTO> UpdateWardAsync(long id, WardUpdateDTO wardDto);
        Task<bool> DeleteWardAsync(long id);
        Task<int> CountWardsAsync(long? districtId = null, long? provinceId = null);
    }
}
