using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class State
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "State Name is required")]
        [StringLength(100, ErrorMessage = "State Name cannot exceed 100 characters")]
        [Display(Name = "State Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم الولاية (عربي)")]
        public string? NameAr { get; set; }

        [StringLength(50, ErrorMessage = "State Code cannot exceed 50 characters")]
        [Display(Name = "State Code")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

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
        public virtual Country? Country { get; set; }
        public virtual ICollection<City>? Cities { get; set; }
    }
}