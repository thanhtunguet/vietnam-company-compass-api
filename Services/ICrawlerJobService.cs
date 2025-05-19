using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;

namespace VietnamBusiness.Services
{
    public interface ICrawlerJobService
    {
        Task<IEnumerable<CrawlerJobDTO>> GetAllCrawlerJobsAsync(string status = null, int? skip = null, int? take = null);
        Task<CrawlerJobDTO> GetCrawlerJobByIdAsync(Guid id);
        Task<CrawlerJobDTO> CreateCrawlerJobAsync(CrawlerJobCreateDTO crawlerJobDto);
        Task<CrawlerJobDTO> UpdateCrawlerJobAsync(Guid id, CrawlerJobUpdateDTO crawlerJobDto);
        Task<bool> DeleteCrawlerJobAsync(Guid id);
        Task<int> CountCrawlerJobsAsync(string status = null);
    }
}
