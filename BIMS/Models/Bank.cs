using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class Bank
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        [StringLength(100, ErrorMessage = "Bank Name cannot exceed 100 characters")]
        [Display(Name = "Bank Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم البنك (عربي)")]
        public string? NameAr { get; set; }

        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [Display(Name = "Bank Code")]
        public string? Code { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Arabic description cannot exceed 500 characters")]
        [Display(Name = "الوصف (عربي)")]
        public string? DescriptionAr { get; set; }

        [StringLength(34, ErrorMessage = "IBAN cannot exceed 34 characters")]
        [Display(Name = "IBAN")]
        public string? IBAN { get; set; }

        [StringLength(11, ErrorMessage = "SWIFT Code cannot exceed 11 characters")]
        [Display(Name = "SWIFT/BIC Code")]
        public string? SwiftCode { get; set; }

        [StringLength(50, ErrorMessage = "Account Number cannot exceed 50 characters")]
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [StringLength(100, ErrorMessage = "Branch Name cannot exceed 100 characters")]
        [Display(Name = "Branch Name (English)")]
        public string? BranchName { get; set; }

        [StringLength(100, ErrorMessage = "Arabic branch name cannot exceed 100 characters")]
        [Display(Name = "اسم الفرع (عربي)")]
        public string? BranchNameAr { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        [Display(Name = "Address (English)")]
        public string? Address { get; set; }

        [StringLength(250, ErrorMessage = "Arabic address cannot exceed 250 characters")]
        [Display(Name = "العنوان (عربي)")]
        public string? AddressAr { get; set; }

        [StringLength(50, ErrorMessage = "Contact Phone cannot exceed 50 characters")]
        [Display(Name = "Contact Phone")]
        public string? ContactPhone { get; set; }

        [StringLength(100, ErrorMessage = "Contact Email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Contact Email")]
        public string? ContactEmail { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }
    }
}