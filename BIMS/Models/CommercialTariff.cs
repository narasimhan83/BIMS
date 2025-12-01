using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Commercial Motor Insurance tariff for a specific insurance plan,
    /// vehicle category, and vehicle type.
    /// </summary>
    public class CommercialTariff
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Vehicle Category is required")]
        [Display(Name = "Vehicle Category")]
        public int VehicleCategoryId { get; set; }

        [Required(ErrorMessage = "Vehicle Type is required")]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Percentage is required")]
        [Range(0, 1000, ErrorMessage = "Percentage must be between 0 and 1000")]
        [Display(Name = "Percentage")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; }

        [Required(ErrorMessage = "Minimum Premium is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum Premium must be non-negative")]
        [Display(Name = "Minimum Premium")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumPremium { get; set; }

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

        [ForeignKey(nameof(VehicleCategoryId))]
        public virtual VehicleCategory? VehicleCategory { get; set; }

        [ForeignKey(nameof(VehicleTypeId))]
        public virtual VehicleType? VehicleType { get; set; }
    }
}