using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    /// <summary>
    /// Master for Excess Types (e.g., Fixed Excess, Percentage Excess).
    /// </summary>
    public class ExcessType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Excess Type is required")]
        [StringLength(100, ErrorMessage = "Excess Type cannot exceed 100 characters")]
        [Display(Name = "Excess Type")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Excess Code is required")]
        [StringLength(50, ErrorMessage = "Excess Code cannot exceed 50 characters")]
        [Display(Name = "Excess Code")]
        public string Code { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}