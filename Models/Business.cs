using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("Business")]
    public class Business
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        [MaxLength(100)]
        public string RootCode { get; set; }

        public long? Vsic2007Id { get; set; }

        [MaxLength(100)]
        public string Vsic2007RootCode { get; set; }

        [MaxLength(100)]
        public string Vsic2007Code { get; set; }

        [MaxLength(500)]
        public string Vsic2007Name { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<CompanyBusinessMapping> CompanyBusinessMappings { get; set; }

        public Business()
        {
            CompanyBusinessMappings = new HashSet<CompanyBusinessMapping>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
