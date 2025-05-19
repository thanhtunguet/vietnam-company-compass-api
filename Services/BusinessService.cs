using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class BusinessService : IBusinessService
    {
        private readonly IRepository<Business> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BusinessService> _logger;

        public BusinessService(IRepository<Business> repository, IMapper mapper, ILogger<BusinessService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BusinessDTO>> GetAllBusinessesAsync(int? skip = null, int? take = null)
        {
            try
            {
                var businesses = await _repository.GetAsync(
                    orderBy: q => q.OrderBy(b => b.Name),
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<BusinessDTO>>(businesses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all businesses");
                throw;
            }
        }

        public async Task<BusinessDTO> GetBusinessByIdAsync(long id)
        {
            try
            {
                var business = await _repository.GetByIdAsync(id);
                if (business == null)
                {
                    _logger.LogWarning("Business with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<BusinessDTO>(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting business with ID {Id}", id);
                throw;
            }
        }

        public async Task<BusinessDTO> GetBusinessByCodeAsync(string code)
        {
            try
            {
                var business = await _repository.GetFirstOrDefaultAsync(b => b.Code == code);
                if (business == null)
                {
                    _logger.LogWarning("Business with Code {Code} not found", code);
                    return null;
                }

                return _mapper.Map<BusinessDTO>(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting business with Code {Code}", code);
                throw;
            }
        }

        public async Task<BusinessDTO> CreateBusinessAsync(BusinessCreateDTO businessDto)
        {
            try
            {
                // Check if business with the same code already exists
                var existingBusiness = await _repository.GetFirstOrDefaultAsync(b => b.Code == businessDto.Code);
                if (existingBusiness != null)
                {
                    _logger.LogWarning("Business with Code {Code} already exists", businessDto.Code);
                    throw new InvalidOperationException($"Business with code {businessDto.Code} already exists");
                }

                var business = _mapper.Map<Business>(businessDto);
                await _repository.AddAsync(business);

                return _mapper.Map<BusinessDTO>(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating business with Code {Code}", businessDto.Code);
                throw;
            }
        }

        public async Task<BusinessDTO> UpdateBusinessAsync(long id, BusinessUpdateDTO businessDto)
        {
            try
            {
                var business = await _repository.GetByIdAsync(id);
                if (business == null)
                {
                    _logger.LogWarning("Business with ID {Id} not found for update", id);
                    return null;
                }

                _mapper.Map(businessDto, business);
                await _repository.UpdateAsync(business);

                return _mapper.Map<BusinessDTO>(business);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating business with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteBusinessAsync(long id)
        {
            try
            {
                var business = await _repository.GetByIdAsync(id);
                if (business == null)
                {
                    _logger.LogWarning("Business with ID {Id} not found for deletion", id);
                    return false;
                }

                // Set DeletedAt instead of deleting
                business.DeletedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(business);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting business with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountBusinessesAsync()
        {
            try
            {
                return await _repository.CountAsync(b => b.DeletedAt == null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting businesses");
                throw;
            }
        }
    }
}
