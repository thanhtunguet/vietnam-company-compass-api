using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class CrawlingStatusDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }

    public class CrawlingStatusCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }

    public class CrawlingStatusUpdateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
