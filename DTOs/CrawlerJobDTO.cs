using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class CrawlerJobDTO
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        public float? Progress { get; set; }

        [MaxLength(100)]
        public string Province { get; set; }

        public int? PageNumber { get; set; }

        [MaxLength(500)]
        public string CompanyUrl { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }

        [MaxLength(255)]
        public string Log { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class CrawlerJobCreateDTO
    {
        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        public float? Progress { get; set; }

        [MaxLength(100)]
        public string Province { get; set; }

        public int? PageNumber { get; set; }

        [MaxLength(500)]
        public string CompanyUrl { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }

        [MaxLength(255)]
        public string Log { get; set; }
    }

    public class CrawlerJobUpdateDTO
    {
        [MaxLength(20)]
        public string Status { get; set; }

        public float? Progress { get; set; }

        public int? PageNumber { get; set; }

        [MaxLength(500)]
        public string CompanyUrl { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }

        [MaxLength(255)]
        public string Log { get; set; }
    }
}
