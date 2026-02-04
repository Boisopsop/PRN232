using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.Account
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name must not exceed 100 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [Range(1, 2, ErrorMessage = "Role must be 1 (Staff) or 2 (Lecturer)")]
        public int AccountRole { get; set; }

        [StringLength(100, ErrorMessage = "Password must not exceed 100 characters")]
        public string? AccountPassword { get; set; }
    }
}
