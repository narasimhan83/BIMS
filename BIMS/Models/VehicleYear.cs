using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class VehicleYear
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [StringLength(50, ErrorMessage = "Year Display cannot exceed 50 characters")]
        [Display(Name = "Year Display (English)")]
        public string? YearDisplay { get; set; }

        [StringLength(50, ErrorMessage = "Arabic year display cannot exceed 50 characters")]
        [Display(Name = "عرض السنة (عربي)")]
        public string? YearDisplayAr { get; set; }

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
    }
}