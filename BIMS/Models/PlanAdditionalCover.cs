using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Links an Insurance Plan to an Additional Cover with fixed and percentage premiums.
    /// </summary>
    public class PlanAdditionalCover
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Additional Cover is required")]
        [Display(Name = "Additional Cover")]
        public int AdditionalCoverId { get; set; }

        [Required(ErrorMessage = "Premium (Fixed) is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Premium (Fixed)")]
        [Range(0, double.MaxValue, ErrorMessage = "Premium (Fixed) must be non-negative")]
        public decimal PremiumFixed { get; set; }

        [Required(ErrorMessage = "Premium (%) is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Premium (%)")]
        [Range(0, double.MaxValue, ErrorMessage = "Premium (%) must be non-negative")]
        public decimal PremiumPercentage { get; set; }

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
        public virtual AdditionalCover? AdditionalCover { get; set; }
    }
}