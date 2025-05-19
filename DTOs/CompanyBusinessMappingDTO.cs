using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class CompanyBusinessMappingDTO
    {
        [Required]
        public long CompanyId { get; set; }

        [Required]
        public long BusinessId { get; set; }

        // For displaying company and business details in responses
        public string CompanyName { get; set; }
        public string BusinessName { get; set; }
    }

    public class CompanyBusinessMappingCreateDTO
    {
        [Required]
        public long CompanyId { get; set; }

        [Required]
        public long BusinessId { get; set; }
    }
}
