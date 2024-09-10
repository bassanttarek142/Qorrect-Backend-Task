using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qorrect_Backend_Task.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(400)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(400)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        [MaxLength(100)]
        public string? Category1 { get; set; }

        [MaxLength(100)]
        public string? Category2 { get; set; }

        [MaxLength(100)]
        public string? Category3 { get; set; }

        [MaxLength(100)]
        public string? Category4 { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime? SubscriptionExpirationDate { get; set; }

        [MaxLength(10)]
        public string? RegistrationCode { get; set; }

        public string? Identifier { get; set; }

        public bool IsVerified { get; set; }

        // Use "Roles" instead of "RoleIds"
        [Required]
        public List<string> Roles { get; set; }

        // New property
        public bool IsActive { get; set; } = true;  // Default to true
    }
}
