using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Country Name is required")]
        [StringLength(100, ErrorMessage = "Country Name cannot exceed 100 characters")]
        [Display(Name = "Country Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم الدولة (عربي)")]
        public string? NameAr { get; set; }

        [StringLength(10, ErrorMessage = "Country Code cannot exceed 10 characters")]
        [Display(Name = "Country Code")]
        [RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "Country Code must be 2-3 uppercase letters (e.g., SA, US, KSA)")]
        public string? Code { get; set; }

        [StringLength(10, ErrorMessage = "Phone Code cannot exceed 10 characters")]
        [Display(Name = "Phone Code")]
        [RegularExpression(@"^\+\d{1,4}$", ErrorMessage = "Phone Code must be in format +XXX (e.g., +966, +1)")]
        public string? PhoneCode { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Arabic description cannot exceed 500 characters")]
        [Display(Name = "الوصف (عربي)")]
        public string? DescriptionAr { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public virtual ICollection<State>? States { get; set; }
    }
}