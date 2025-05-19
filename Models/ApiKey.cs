using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("ApiKeys")]
    public class ApiKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Key { get; set; }

        [MaxLength(50)]
        public string Plan { get; set; }

        public int RequestLimit { get; set; }

        public int RequestsUsed { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public virtual ICollection<ApiUsageTracking> ApiUsageTrackings { get; set; }

        public ApiKey()
        {
            ApiUsageTrackings = new HashSet<ApiUsageTracking>();
            Key = Guid.NewGuid().ToString("N");
            Plan = "free";
            RequestLimit = 500;
            RequestsUsed = 0;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
