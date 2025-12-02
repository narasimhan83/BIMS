using System;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class LeadEnquiryVehicle
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lead Enquiry")]
        public int LeadEnquiryId { get; set; }

        // Vehicle details
        [StringLength(50)]
        [Display(Name = "Registration Number")]
        public string? RegistrationNumber { get; set; }

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

        // Motor-specific quote fields
        [Display(Name = "Expected Premium")]
        [Range(0, double.MaxValue, ErrorMessage = "Expected premium must be positive")]
        public decimal? ExpectedPremium { get; set; }

        [Display(Name = "Policy Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? PolicyExpiryDate { get; set; }

        // Navigation properties
        public virtual LeadEnquiry? LeadEnquiry { get; set; }
        public virtual VehicleMake? VehicleMake { get; set; }
        public virtual VehicleModel? VehicleModel { get; set; }
        public virtual VehicleYear? VehicleYear { get; set; }
        public virtual EngineCapacity? EngineCapacity { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
    }
}