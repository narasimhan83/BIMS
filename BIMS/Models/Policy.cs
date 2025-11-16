using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class Policy
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Policy number is required")]
        [Display(Name = "Policy Number")]
        [StringLength(50, ErrorMessage = "Policy number cannot exceed 50 characters")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Insurance client is required")]
        [Display(Name = "Insurance Client")]
        public int InsuranceClientId { get; set; }

        [Display(Name = "Insurance Client")]
        public virtual InsuranceClient InsuranceClient { get; set; }

        [Required(ErrorMessage = "Product type is required")]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Product Type")]
        public virtual ProductType ProductType { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Premium amount is required")]
        [Display(Name = "Premium Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Premium amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active"; // Active, Expired, Cancelled, Claimed

        [Display(Name = "Coverage Details (English)")]
        [StringLength(2000, ErrorMessage = "Coverage details cannot exceed 2000 characters")]
        public string CoverageDetailsEn { get; set; }

        [Display(Name = "Coverage Details (Arabic)")]
        [StringLength(2000, ErrorMessage = "Coverage details cannot exceed 2000 characters")]
        public string CoverageDetailsAr { get; set; }

        [Required(ErrorMessage = "Sum insured is required")]
        [Display(Name = "Sum Insured")]
        [Range(0, double.MaxValue, ErrorMessage = "Sum insured must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SumInsured { get; set; }

        [Display(Name = "Terms and Conditions (English)")]
        [StringLength(2000, ErrorMessage = "Terms and conditions cannot exceed 2000 characters")]
        public string TermsAndConditionsEn { get; set; }

        [Display(Name = "Terms and Conditions (Arabic)")]
        [StringLength(2000, ErrorMessage = "Terms and conditions cannot exceed 2000 characters")]
        public string TermsAndConditionsAr { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}