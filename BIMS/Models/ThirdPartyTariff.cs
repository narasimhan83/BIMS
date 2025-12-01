using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Third Party Motor Insurance tariff for a specific insurance plan,
    /// engine capacity range, and vehicle type.
    /// </summary>
    public class ThirdPartyTariff
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Engine Capacity Range is required")]
        [Display(Name = "Engine Capacity Range")]
        public int EngineCapacityId { get; set; }

        [Required(ErrorMessage = "Vehicle Type is required")]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Premium Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Premium Amount must be non-negative")]
        [Display(Name = "Premium Amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumAmount { get; set; }

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

        [ForeignKey(nameof(EngineCapacityId))]
        public virtual EngineCapacity? EngineCapacity { get; set; }

        [ForeignKey(nameof(VehicleTypeId))]
        public virtual VehicleType? VehicleType { get; set; }
    }
}