using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietnamBusiness.DTOs;
using VietnamBusiness.Models;
using VietnamBusiness.Repositories;

namespace VietnamBusiness.Services
{
    public class CrawlerJobService : ICrawlerJobService
    {
        private readonly IRepository<CrawlerJob> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CrawlerJobService> _logger;

        public CrawlerJobService(IRepository<CrawlerJob> repository, IMapper mapper, ILogger<CrawlerJobService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CrawlerJobDTO>> GetAllCrawlerJobsAsync(string status = null, int? skip = null, int? take = null)
        {
            try
            {
                var jobs = await _repository.GetAsync(
                    filter: string.IsNullOrEmpty(status) ? null : j => j.Status == status,
                    orderBy: q => q.OrderByDescending(j => j.CreatedAt),
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<CrawlerJobDTO>>(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting crawler jobs");
                throw;
            }
        }

        public async Task<CrawlerJobDTO> GetCrawlerJobByIdAsync(Guid id)
        {
            try
            {
                var job = await _repository.GetByIdAsync(id);
                if (job == null)
                {
                    _logger.LogWarning("CrawlerJob with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<CrawlerJobDTO>(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting crawler job with ID {Id}", id);
                throw;
            }
        }

        public async Task<CrawlerJobDTO> CreateCrawlerJobAsync(CrawlerJobCreateDTO crawlerJobDto)
        {
            try
            {
                var job = _mapper.Map<CrawlerJob>(crawlerJobDto);
                await _repository.AddAsync(job);

                return _mapper.Map<CrawlerJobDTO>(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating crawler job");
                throw;
            }
        }

        public async Task<CrawlerJobDTO> UpdateCrawlerJobAsync(Guid id, CrawlerJobUpdateDTO crawlerJobDto)
        {
            try
            {
                var job = await _repository.GetByIdAsync(id);
                if (job == null)
                {
                    _logger.LogWarning("CrawlerJob with ID {Id} not found for update", id);
                    return null;
                }

                _mapper.Map(crawlerJobDto, job);
                await _repository.UpdateAsync(job);

                return _mapper.Map<CrawlerJobDTO>(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating crawler job with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCrawlerJobAsync(Guid id)
        {
            try
            {
                var job = await _repository.GetByIdAsync(id);
                if (job == null)
                {
                    _logger.LogWarning("CrawlerJob with ID {Id} not found for deletion", id);
                    return false;
                }

                await _repository.DeleteAsync(job);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting crawler job with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountCrawlerJobsAsync(string status = null)
        {
            try
            {
                return await _repository.CountAsync(
                    string.IsNullOrEmpty(status) ? null : j => j.Status == status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting crawler jobs");
                throw;
            }
        }
    }
}
