using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.Requests
{
    public class CreateSystemAccountRequest
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

        [Required]
        [StringLength(70)]
        public string AccountPassword { get; set; } = string.Empty;
    }

    public class UpdateSystemAccountRequest
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
