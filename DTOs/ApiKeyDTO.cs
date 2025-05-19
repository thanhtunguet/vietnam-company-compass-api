using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class ApiKeyDTO
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Key { get; set; }

        [MaxLength(50)]
        public string Plan { get; set; }

        public int RequestLimit { get; set; }

        public int RequestsUsed { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // For displaying user details in responses
        public string UserEmail { get; set; }
    }

    public class ApiKeyCreateDTO
    {
        [Required]
        public long UserId { get; set; }

        [MaxLength(50)]
        public string Plan { get; set; } = "free";

        public int RequestLimit { get; set; } = 500;

        public bool IsActive { get; set; } = true;
    }

    public class ApiKeyUpdateDTO
    {
        [MaxLength(50)]
        public string Plan { get; set; }

        public int? RequestLimit { get; set; }

        public int? RequestsUsed { get; set; }

        public bool? IsActive { get; set; }
    }
}
