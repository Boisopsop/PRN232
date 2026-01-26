using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.DTOs
{
    public class SystemAccountDTO
    {
        public short AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(70)]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        public int? AccountRole { get; set; }

        [Required]
        [StringLength(70)]
        public string AccountPassword { get; set; } = string.Empty;
    }

    public class UpdateAccountDTO
    {
        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(70)]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        public int? AccountRole { get; set; }

        [StringLength(70)]
        public string? AccountPassword { get; set; }
    }
}
