using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.Tag
{
    public class UpdateTagRequest
    {
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, ErrorMessage = "Tag name must not exceed 50 characters")]
        public string TagName { get; set; } = string.Empty;

        [StringLength(400, ErrorMessage = "Note must not exceed 400 characters")]
        public string Note { get; set; } = string.Empty;
    }
}
