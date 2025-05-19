using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("CompanyStatus")]
    public class CompanyStatus
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<Company> Companies { get; set; }

        public CompanyStatus()
        {
            Companies = new HashSet<Company>();
        }
    }
}
