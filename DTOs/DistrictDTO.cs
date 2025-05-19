using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class DistrictDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Type { get; set; }

        [Required]
        public long ProvinceId { get; set; }

        [MaxLength(500)]
        public string EnglishName { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        // For displaying province details in responses
        public string ProvinceName { get; set; }
    }

    public class DistrictCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Type { get; set; }

        [Required]
        public long ProvinceId { get; set; }

        [MaxLength(500)]
        public string EnglishName { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }
    }

    public class DistrictUpdateDTO
    {
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Type { get; set; }

        public long? ProvinceId { get; set; }

        [MaxLength(500)]
        public string EnglishName { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }
    }
}
