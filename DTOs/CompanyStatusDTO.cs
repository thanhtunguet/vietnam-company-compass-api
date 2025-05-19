using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class CompanyStatusDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }

    public class CompanyStatusCreateDTO
    {
        [Required]
        [MaxLength(255)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }

    public class CompanyStatusUpdateDTO
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
