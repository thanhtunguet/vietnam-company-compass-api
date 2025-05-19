using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class ApiUsageTrackingDTO
    {
        public long Id { get; set; }

        public long ApiKeyId { get; set; }

        [MaxLength(255)]
        public string Endpoint { get; set; }

        public int? ResponseTime { get; set; }

        public int? StatusCode { get; set; }

        public DateTime CalledAt { get; set; }

        // For displaying API key details in responses
        public string ApiKey { get; set; }
        public string UserEmail { get; set; }
    }

    public class ApiUsageTrackingCreateDTO
    {
        [Required]
        public long ApiKeyId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Endpoint { get; set; }

        public int? ResponseTime { get; set; }

        public int? StatusCode { get; set; }
    }
}
