using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Links an Insurance Plan to a Special Condition with loading percentage.
    /// </summary>
    public class PlanCondition
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Special Condition is required")]
        [Display(Name = "Special Condition")]
        public int SpecialConditionId { get; set; }

        [Required(ErrorMessage = "Loading Percentage is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Loading Percentage")]
        [Range(0, double.MaxValue, ErrorMessage = "Loading Percentage must be non-negative")]
        public decimal LoadingPercentage { get; set; }

        [Required(ErrorMessage = "Applies When is required")]
        [StringLength(500, ErrorMessage = "Applies When cannot exceed 500 characters")]
        [Display(Name = "Applies When")]
        public string AppliesWhen { get; set; } = string.Empty;

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

        // Navigation properties
        public virtual InsurancePlan? InsurancePlan { get; set; }
        public virtual SpecialCondition? SpecialCondition { get; set; }
    }
}