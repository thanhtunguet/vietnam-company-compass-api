using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface IBusinessService
    {
        Task<IEnumerable<BusinessDTO>> GetAllBusinessesAsync(int? skip = null, int? take = null);
        Task<BusinessDTO> GetBusinessByIdAsync(long id);
        Task<BusinessDTO> GetBusinessByCodeAsync(string code);
        Task<BusinessDTO> CreateBusinessAsync(BusinessCreateDTO businessDto);
        Task<BusinessDTO> UpdateBusinessAsync(long id, BusinessUpdateDTO businessDto);
        Task<bool> DeleteBusinessAsync(long id);
        Task<int> CountBusinessesAsync();
    }
}
