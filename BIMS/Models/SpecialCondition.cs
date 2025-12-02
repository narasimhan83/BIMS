using System;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    /// <summary>
    /// Defines a special condition under motor insurance.
    /// </summary>
    public class SpecialCondition
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Condition Code is required")]
        [StringLength(50, ErrorMessage = "Condition Code cannot exceed 50 characters")]
        [Display(Name = "Condition Code")]
        public string ConditionCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [StringLength(50)]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }
    }
}