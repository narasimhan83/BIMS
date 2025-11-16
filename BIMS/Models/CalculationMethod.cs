using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class CalculationMethod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Method Name is required")]
        [StringLength(100, ErrorMessage = "Method Name cannot exceed 100 characters")]
        [Display(Name = "Method Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم طريقة الحساب (عربي)")]
        public string? NameAr { get; set; }

        [StringLength(50, ErrorMessage = "Method Code cannot exceed 50 characters")]
        [Display(Name = "Method Code")]
        public string? Code { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Arabic description cannot exceed 500 characters")]
        [Display(Name = "الوصف (عربي)")]
        public string? DescriptionAr { get; set; }

        [StringLength(1000, ErrorMessage = "Formula cannot exceed 1000 characters")]
        [Display(Name = "Calculation Formula")]
        public string? Formula { get; set; }

        [StringLength(1000, ErrorMessage = "Arabic formula cannot exceed 1000 characters")]
        [Display(Name = "صيغة الحساب (عربي)")]
        public string? FormulaAr { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}