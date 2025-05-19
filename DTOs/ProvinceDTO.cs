using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class ProvinceDTO
    {
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
    }

    public class ProvinceCreateDTO
    {
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
    }

    public class ProvinceUpdateDTO
    {
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Type { get; set; }

        [MaxLength(500)]
        public string EnglishName { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }
    }
}
