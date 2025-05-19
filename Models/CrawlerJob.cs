using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("CrawlerJob")]
    public class CrawlerJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public CrawlerJob()
        {
            Id = Guid.NewGuid();
            Progress = 0.0f;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
