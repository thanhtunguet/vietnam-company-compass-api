using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class CompanyDTO
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public string TaxCode { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Representative { get; set; }

        [MaxLength(500)]
        public string MainBusiness { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string FormattedAddress { get; set; }

        public DateTime? IssuedAt { get; set; }

        [MaxLength(500)]
        public string CurrentStatus { get; set; }

        [MaxLength(500)]
        public string AlternateName { get; set; }

        [MaxLength(2048)]
        public string Slug { get; set; }

        public bool? IsCrawledFull { get; set; }

        public long? ProvinceId { get; set; }

        public long? DistrictId { get; set; }

        public long? WardId { get; set; }

        public long? MainBusinessId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime? TerminatedAt { get; set; }

        public long? StatusId { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        // For displaying location details in responses
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string StatusName { get; set; }
    }

    public class CompanyCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string TaxCode { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Representative { get; set; }

        [MaxLength(500)]
        public string MainBusiness { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string FormattedAddress { get; set; }

        public DateTime? IssuedAt { get; set; }

        [MaxLength(500)]
        public string CurrentStatus { get; set; }

        [MaxLength(500)]
        public string AlternateName { get; set; }

        [MaxLength(2048)]
        public string Slug { get; set; }

        public bool? IsCrawledFull { get; set; }

        public long? ProvinceId { get; set; }

        public long? DistrictId { get; set; }

        public long? WardId { get; set; }

        public long? MainBusinessId { get; set; }

        public DateTime? TerminatedAt { get; set; }

        public long? StatusId { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }
    }

    public class CompanyUpdateDTO
    {
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Representative { get; set; }

        [MaxLength(500)]
        public string MainBusiness { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string FormattedAddress { get; set; }

        public DateTime? IssuedAt { get; set; }

        [MaxLength(500)]
        public string CurrentStatus { get; set; }

        [MaxLength(500)]
        public string AlternateName { get; set; }

        [MaxLength(2048)]
        public string Slug { get; set; }

        public bool? IsCrawledFull { get; set; }

        public long? ProvinceId { get; set; }

        public long? DistrictId { get; set; }

        public long? WardId { get; set; }

        public long? MainBusinessId { get; set; }

        public DateTime? TerminatedAt { get; set; }

        public long? StatusId { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
