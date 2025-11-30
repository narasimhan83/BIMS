using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class ComprehensiveTariff
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Value Band is required")]
        [Display(Name = "Value Band")]
        public int ValueBandId { get; set; }

        [Required(ErrorMessage = "Vehicle Category is required")]
        [Display(Name = "Vehicle Category")]
        public int VehicleCategoryId { get; set; }

        [Required(ErrorMessage = "Vehicle Type is required")]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Percentage is required")]
        [Range(0, 1000, ErrorMessage = "Percentage must be between 0 and 1000")]
        [Display(Name = "Percentage")]
        public decimal Percentage { get; set; }

        [Required(ErrorMessage = "Minimum Premium is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum Premium must be non-negative")]
        [Display(Name = "Minimum Premium")]
        public decimal MinimumPremium { get; set; }

        [Required(ErrorMessage = "Excess is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Excess must be non-negative")]
        [Display(Name = "Excess")]
        public decimal Excess { get; set; }

        [Required(ErrorMessage = "Effective From date is required")]
        [Display(Name = "Effective From")]
        [DataType(DataType.Date)]
        public DateTime EffectiveFrom { get; set; }

        [Display(Name = "Effective To")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveTo { get; set; }

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
        [ForeignKey(nameof(InsurancePlanId))]
        public virtual InsurancePlan? InsurancePlan { get; set; }

        [ForeignKey(nameof(ValueBandId))]
        public virtual ValueBand? ValueBand { get; set; }

        [ForeignKey(nameof(VehicleCategoryId))]
        public virtual VehicleCategory? VehicleCategory { get; set; }

        [ForeignKey(nameof(VehicleTypeId))]
        public virtual VehicleType? VehicleType { get; set; }
    }
}