using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class BusinessDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        [MaxLength(100)]
        public string RootCode { get; set; }

        public long? Vsic2007Id { get; set; }

        [MaxLength(100)]
        public string Vsic2007RootCode { get; set; }

        [MaxLength(100)]
        public string Vsic2007Code { get; set; }

        [MaxLength(500)]
        public string Vsic2007Name { get; set; }
    }

    public class BusinessCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string RootCode { get; set; }

        public long? Vsic2007Id { get; set; }

        [MaxLength(100)]
        public string Vsic2007RootCode { get; set; }

        [MaxLength(100)]
        public string Vsic2007Code { get; set; }

        [MaxLength(500)]
        public string Vsic2007Name { get; set; }
    }

    public class BusinessUpdateDTO
    {
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string RootCode { get; set; }

        public long? Vsic2007Id { get; set; }

        [MaxLength(100)]
        public string Vsic2007RootCode { get; set; }

        [MaxLength(100)]
        public string Vsic2007Code { get; set; }

        [MaxLength(500)]
        public string Vsic2007Name { get; set; }
    }
}
