using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("Province")]
    public class Province
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Type { get; set; }

        [MaxLength(500)]
        public string EnglishName { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
        public virtual ICollection<Company> Companies { get; set; }

        public Province()
        {
            Districts = new HashSet<District>();
            Wards = new HashSet<Ward>();
            Companies = new HashSet<Company>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
