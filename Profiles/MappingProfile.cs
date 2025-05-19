using AutoMapper;
using VietnamBusiness.DTOs;
using VietnamBusiness.Models;

namespace VietnamBusiness.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Business mappings
            CreateMap<Business, BusinessDTO>();
            CreateMap<BusinessCreateDTO, Business>();
            CreateMap<BusinessUpdateDTO, Business>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // CompanyStatus mappings
            CreateMap<CompanyStatus, CompanyStatusDTO>();
            CreateMap<CompanyStatusCreateDTO, CompanyStatus>();
            CreateMap<CompanyStatusUpdateDTO, CompanyStatus>();

            // CrawlerJob mappings
            CreateMap<CrawlerJob, CrawlerJobDTO>();
            CreateMap<CrawlerJobCreateDTO, CrawlerJob>();
            CreateMap<CrawlerJobUpdateDTO, CrawlerJob>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // CrawlingStatus mappings
            CreateMap<CrawlingStatus, CrawlingStatusDTO>();
            CreateMap<CrawlingStatusCreateDTO, CrawlingStatus>();
            CreateMap<CrawlingStatusUpdateDTO, CrawlingStatus>();

            // Province mappings
            CreateMap<Province, ProvinceDTO>();
            CreateMap<ProvinceCreateDTO, Province>();
            CreateMap<ProvinceUpdateDTO, Province>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // District mappings
            CreateMap<District, DistrictDTO>()
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province != null ? src.Province.Name : null));
            CreateMap<DistrictCreateDTO, District>();
            CreateMap<DistrictUpdateDTO, District>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // User mappings
            CreateMap<User, UserDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ApiKey mappings
            CreateMap<ApiKey, ApiKeyDTO>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));
            CreateMap<ApiKeyCreateDTO, ApiKey>();
            CreateMap<ApiKeyUpdateDTO, ApiKey>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ApiUsageTracking mappings
            CreateMap<ApiUsageTracking, ApiUsageTrackingDTO>()
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => src.ApiKey != null ? src.ApiKey.Key : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.ApiKey != null && src.ApiKey.User != null ? src.ApiKey.User.Email : null));
            CreateMap<ApiUsageTrackingCreateDTO, ApiUsageTracking>();

            // Ward mappings
            CreateMap<Ward, WardDTO>()
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province != null ? src.Province.Name : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null));
            CreateMap<WardCreateDTO, Ward>();
            CreateMap<WardUpdateDTO, Ward>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Company mappings
            CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province != null ? src.Province.Name : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null))
                .ForMember(dest => dest.WardName, opt => opt.MapFrom(src => src.Ward != null ? src.Ward.Name : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.Name : null));
            CreateMap<CompanyCreateDTO, Company>();
            CreateMap<CompanyUpdateDTO, Company>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // CompanyBusinessMapping mappings
            CreateMap<CompanyBusinessMapping, CompanyBusinessMappingDTO>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.Business != null ? src.Business.Name : null));
            CreateMap<CompanyBusinessMappingCreateDTO, CompanyBusinessMapping>();
        }
    }
}
