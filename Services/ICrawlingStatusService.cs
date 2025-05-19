using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface ICrawlingStatusService
    {
        Task<IEnumerable<CrawlingStatusDTO>> GetAllCrawlingStatusesAsync();
        Task<CrawlingStatusDTO> GetCrawlingStatusByIdAsync(long id);
        Task<CrawlingStatusDTO> GetCrawlingStatusByCodeAsync(string code);
        Task<CrawlingStatusDTO> CreateCrawlingStatusAsync(CrawlingStatusCreateDTO crawlingStatusDto);
        Task<CrawlingStatusDTO> UpdateCrawlingStatusAsync(long id, CrawlingStatusUpdateDTO crawlingStatusDto);
        Task<bool> DeleteCrawlingStatusAsync(long id);
    }
}
