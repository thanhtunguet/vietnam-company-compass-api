using System;
using System.ComponentModel.DataAnnotations;

namespace VietnamBusiness.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class UserCreateDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UserUpdateDTO
    {
        [MaxLength(255)]
        public string Name { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UserPasswordUpdateDTO
    {
        [Required]
        [MinLength(6)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [MinLength(6)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
