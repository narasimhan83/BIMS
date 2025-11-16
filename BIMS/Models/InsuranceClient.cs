using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class InsuranceClient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client Name is required")]
        [StringLength(100, ErrorMessage = "Client Name cannot exceed 100 characters")]
        [Display(Name = "Client Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم العميل (عربي)")]
        public string? NameAr { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        [Display(Name = "Phone")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]+$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Mobile cannot exceed 20 characters")]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]+$", ErrorMessage = "Please enter a valid mobile number")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public virtual Country? Country { get; set; }
        public virtual State? State { get; set; }
        public virtual City? City { get; set; }
        public virtual ICollection<InsuranceClientBankDetail>? BankDetails { get; set; }
        public virtual ICollection<LineOfBusiness>? LinesOfBusiness { get; set; }
    }
}