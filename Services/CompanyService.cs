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
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(IRepository<Company> repository, IMapper mapper, ILogger<CompanyService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(int? skip = null, int? take = null)
        {
            try
            {
                var companies = await _repository.GetAsync(
                    filter: c => c.DeletedAt == null,
                    orderBy: q => q.OrderBy(c => c.Name),
                    includeProperties: "Province,District,Ward,Status",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all companies");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompaniesByProvinceIdAsync(long provinceId, int? skip = null, int? take = null)
        {
            try
            {
                var companies = await _repository.GetAsync(
                    filter: c => c.ProvinceId == provinceId && c.DeletedAt == null,
                    orderBy: q => q.OrderBy(c => c.Name),
                    includeProperties: "Province,District,Ward,Status",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting companies for province ID {ProvinceId}", provinceId);
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompaniesByDistrictIdAsync(long districtId, int? skip = null, int? take = null)
        {
            try
            {
                var companies = await _repository.GetAsync(
                    filter: c => c.DistrictId == districtId && c.DeletedAt == null,
                    orderBy: q => q.OrderBy(c => c.Name),
                    includeProperties: "Province,District,Ward,Status",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting companies for district ID {DistrictId}", districtId);
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompaniesByWardIdAsync(long wardId, int? skip = null, int? take = null)
        {
            try
            {
                var companies = await _repository.GetAsync(
                    filter: c => c.WardId == wardId && c.DeletedAt == null,
                    orderBy: q => q.OrderBy(c => c.Name),
                    includeProperties: "Province,District,Ward,Status",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting companies for ward ID {WardId}", wardId);
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompaniesByStatusIdAsync(long statusId, int? skip = null, int? take = null)
        {
            try
            {
                var companies = await _repository.GetAsync(
                    filter: c => c.StatusId == statusId && c.DeletedAt == null,
                    orderBy: q => q.OrderBy(c => c.Name),
                    includeProperties: "Province,District,Ward,Status",
                    skip: skip,
                    take: take);

                return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting companies for status ID {StatusId}", statusId);
                throw;
            }
        }

        public async Task<CompanyDTO> GetCompanyByIdAsync(long id)
        {
            try
            {
                var company = await _repository.GetFirstOrDefaultAsync(
                    filter: c => c.Id == id && c.DeletedAt == null,
                    includeProperties: "Province,District,Ward,Status");

                if (company == null)
                {
                    _logger.LogWarning("Company with ID {Id} not found or deleted", id);
                    return null;
                }

                return _mapper.Map<CompanyDTO>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company with ID {Id}", id);
                throw;
            }
        }

        public async Task<CompanyDTO> GetCompanyByTaxCodeAsync(string taxCode)
        {
            try
            {
                var company = await _repository.GetFirstOrDefaultAsync(
                    filter: c => c.TaxCode == taxCode && c.DeletedAt == null,
                    includeProperties: "Province,District,Ward,Status");

                if (company == null)
                {
                    _logger.LogWarning("Company with tax code {TaxCode} not found or deleted", taxCode);
                    return null;
                }

                return _mapper.Map<CompanyDTO>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company with tax code {TaxCode}", taxCode);
                throw;
            }
        }

        public async Task<CompanyDTO> CreateCompanyAsync(CompanyCreateDTO companyDto)
        {
            try
            {
                if (!string.IsNullOrEmpty(companyDto.TaxCode))
                {
                    var existingCompany = await _repository.GetFirstOrDefaultAsync(c => c.TaxCode == companyDto.TaxCode);
                    if (existingCompany != null)
                    {
                        _logger.LogWarning("Company with tax code {TaxCode} already exists", companyDto.TaxCode);
                        throw new InvalidOperationException($"Company with tax code {companyDto.TaxCode} already exists");
                    }
                }

                var company = _mapper.Map<Company>(companyDto);
                await _repository.AddAsync(company);

                // Reload the company with location and status details
                company = await _repository.GetFirstOrDefaultAsync(
                    filter: c => c.Id == company.Id,
                    includeProperties: "Province,District,Ward,Status");

                return _mapper.Map<CompanyDTO>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company with tax code {TaxCode}", companyDto.TaxCode);
                throw;
            }
        }

        public async Task<CompanyDTO> UpdateCompanyAsync(long id, CompanyUpdateDTO companyDto)
        {
            try
            {
                var company = await _repository.GetFirstOrDefaultAsync(
                    filter: c => c.Id == id && c.DeletedAt == null);

                if (company == null)
                {
                    _logger.LogWarning("Company with ID {Id} not found or deleted for update", id);
                    return null;
                }

                _mapper.Map(companyDto, company);
                await _repository.UpdateAsync(company);

                // Reload the company with location and status details
                company = await _repository.GetFirstOrDefaultAsync(
                    filter: c => c.Id == id,
                    includeProperties: "Province,District,Ward,Status");

                return _mapper.Map<CompanyDTO>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCompanyAsync(long id)
        {
            try
            {
                var company = await _repository.GetByIdAsync(id);
                if (company == null || company.DeletedAt != null)
                {
                    _logger.LogWarning("Company with ID {Id} not found or already deleted", id);
                    return false;
                }

                // Soft delete
                company.DeletedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(company);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting company with ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CountCompaniesAsync(long? provinceId = null, long? districtId = null, long? wardId = null, long? statusId = null)
        {
            try
            {
                if (provinceId.HasValue)
                {
                    return await _repository.CountAsync(
                        c => c.ProvinceId == provinceId.Value && c.DeletedAt == null);
                }
                else if (districtId.HasValue)
                {
                    return await _repository.CountAsync(
                        c => c.DistrictId == districtId.Value && c.DeletedAt == null);
                }
                else if (wardId.HasValue)
                {
                    return await _repository.CountAsync(
                        c => c.WardId == wardId.Value && c.DeletedAt == null);
                }
                else if (statusId.HasValue)
                {
                    return await _repository.CountAsync(
                        c => c.StatusId == statusId.Value && c.DeletedAt == null);
                }
                else
                {
                    return await _repository.CountAsync(c => c.DeletedAt == null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting companies");
                throw;
            }
        }
    }
}
