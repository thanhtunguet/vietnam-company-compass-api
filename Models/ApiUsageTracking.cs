using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("ApiUsageTracking")]
    public class ApiUsageTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ApiKeyId { get; set; }

        [MaxLength(255)]
        public string Endpoint { get; set; }

        public int? ResponseTime { get; set; }

        public int? StatusCode { get; set; }

        public DateTime CalledAt { get; set; }

        // Navigation property
        [ForeignKey(nameof(ApiKeyId))]
        public virtual ApiKey ApiKey { get; set; }

        public ApiUsageTracking()
        {
            CalledAt = DateTime.UtcNow;
            // Initialize required properties
            Endpoint = string.Empty;
        }
    }
}
