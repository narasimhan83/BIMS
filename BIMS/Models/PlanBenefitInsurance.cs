using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Links an Insurance Plan to a Benefit Type with coverage, limits, and excess.
    /// </summary>
    public class PlanBenefits
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int? InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Benefit Type is required")]
        [Display(Name = "Benefit Type")]
        public int? BenefitTypeId { get; set; }

        [Display(Name = "Is Covered")]
        public bool? IsCovered { get; set; } = true;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Limit Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Limit Amount must be non-negative")]
        public decimal? LimitAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Excess Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Excess Amount must be non-negative")]
        public decimal? ExcessAmount { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        [Display(Name = "Active")]
        public bool? IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [StringLength(50)]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        // Navigation properties
        [ForeignKey(nameof(InsurancePlanId))]
        public virtual InsurancePlan? InsurancePlan { get; set; }

        [ForeignKey(nameof(BenefitTypeId))]
        public virtual BenefitType? BenefitType { get; set; }
    }
}