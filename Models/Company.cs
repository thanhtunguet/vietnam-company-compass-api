using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
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
        
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        
        [MaxLength(100)]
        public string Email { get; set; }
        
        [MaxLength(255)]
        public string Website { get; set; }
        
        [MaxLength(255)]
        public string LegalRepresentative { get; set; }
        
        [MaxLength(100)]
        public string BusinessType { get; set; }
        
        public int? FoundedYear { get; set; }
        
        public int? EmployeeCount { get; set; }
        
        public decimal? Capital { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ProvinceId))]
        public virtual Province Province { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }

        [ForeignKey(nameof(WardId))]
        public virtual Ward Ward { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual CompanyStatus Status { get; set; }

        public virtual ICollection<CompanyBusinessMapping> CompanyBusinessMappings { get; set; }

        public Company()
        {
            CompanyBusinessMappings = new HashSet<CompanyBusinessMapping>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
