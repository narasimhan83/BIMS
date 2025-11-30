using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class VehicleCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vehicle Category Name is required")]
        [StringLength(100, ErrorMessage = "Vehicle Category Name cannot exceed 100 characters")]
        [Display(Name = "Vehicle Category Name (English)")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم فئة المركبة (عربي)")]
        public string? CategoryNameAr { get; set; }

        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [Display(Name = "Code")]
        public string? Code { get; set; }

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

        [StringLength(50)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [StringLength(50)]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }
    }
}