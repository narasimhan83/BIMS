using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class CustomerVehicle
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        // Vehicle Information (Foreign Keys)
        [Display(Name = "Vehicle Make")]
        public int? VehicleMakeId { get; set; }

        [Display(Name = "Vehicle Model")]
        public int? VehicleModelId { get; set; }

        [Display(Name = "Vehicle Year")]
        public int? VehicleYearId { get; set; }

        [Display(Name = "Engine Capacity")]
        public int? EngineCapacityId { get; set; }

        [Display(Name = "Vehicle Type")]
        public int? VehicleTypeId { get; set; }

        // Vehicle Specific Details
        [StringLength(50)]
        [Display(Name = "Registration Number")]
        public string? RegistrationNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Chassis Number")]
        public string? ChassisNumber { get; set; }

        [StringLength(500)]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(500)]
        [Display(Name = "الوصف (عربي)")]
        public string? DescriptionAr { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("VehicleMakeId")]
        public virtual VehicleMake? VehicleMake { get; set; }

        [ForeignKey("VehicleModelId")]
        public virtual VehicleModel? VehicleModel { get; set; }

        [ForeignKey("VehicleYearId")]
        public virtual VehicleYear? VehicleYear { get; set; }

        [ForeignKey("EngineCapacityId")]
        public virtual EngineCapacity? EngineCapacity { get; set; }

        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType? VehicleType { get; set; }
    }
}