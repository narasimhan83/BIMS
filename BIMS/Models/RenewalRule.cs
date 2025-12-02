using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Defines renewal rules per insurance plan and line of business.
    /// </summary>
    public class RenewalRule
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Line of Business is required")]
        [Display(Name = "Line of Business")]
        public int LineOfBusinessId { get; set; }

        [Required(ErrorMessage = "Claims Count is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Claims Count must be non-negative")]
        [Display(Name = "Claims Count")]
        public int ClaimsCount { get; set; }

        [Required(ErrorMessage = "Loading Percentage is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Loading Percentage must be non-negative")]
        [Display(Name = "Loading Percentage (%)")]
        public decimal LoadingPercentage { get; set; }

        [Required(ErrorMessage = "Minimum Premium is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum Premium must be non-negative")]
        [Display(Name = "Minimum Premium")]
        public decimal MinimumPremium { get; set; }

        [Required(ErrorMessage = "Action is required")]
        [StringLength(100, ErrorMessage = "Action cannot exceed 100 characters")]
        [Display(Name = "Action")]
        public string Action { get; set; } = string.Empty;

        [Required(ErrorMessage = "Effective Year is required")]
        [Range(1900, 2100, ErrorMessage = "Effective Year must be between 1900 and 2100")]
        [Display(Name = "Effective Year")]
        public int EffectiveYear { get; set; }

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

        // Navigation
        public virtual InsurancePlan? InsurancePlan { get; set; }
        public virtual LineOfBusiness? LineOfBusiness { get; set; }
    }
}