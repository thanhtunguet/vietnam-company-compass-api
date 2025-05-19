using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IProvinceService
    {
        Task<IEnumerable<ProvinceDTO>> GetAllProvincesAsync(int? skip = null, int? take = null);
        Task<ProvinceDTO> GetProvinceByIdAsync(long id);
        Task<ProvinceDTO> GetProvinceByCodeAsync(string code);
        Task<ProvinceDTO> CreateProvinceAsync(ProvinceCreateDTO provinceDto);
        Task<ProvinceDTO> UpdateProvinceAsync(long id, ProvinceUpdateDTO provinceDto);
        Task<bool> DeleteProvinceAsync(long id);
        Task<int> CountProvincesAsync();
    }
}
