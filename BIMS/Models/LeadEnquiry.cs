using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class LeadEnquiry
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lead")]
        public int LeadId { get; set; }

        [Required(ErrorMessage = "Product Class is required")]
        [Display(Name = "Product Class")]
        public int ProductClassId { get; set; }

        [Required(ErrorMessage = "Product Type is required")]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Primary Enquiry")]
        public bool IsPrimary { get; set; }

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        // Navigation properties
        public virtual Lead? Lead { get; set; }
        public virtual ProductClass? ProductClass { get; set; }
        public virtual ProductType? ProductType { get; set; }
        public virtual ICollection<LeadEnquiryVehicle> Vehicles { get; set; } = new List<LeadEnquiryVehicle>();
    }
}