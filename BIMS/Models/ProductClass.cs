using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class ProductClass
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Business Type is required")]
        [Display(Name = "Business Type")]
        public int BusinessTypeId { get; set; }

        [Required(ErrorMessage = "Product Class Name is required")]
        [StringLength(100, ErrorMessage = "Product Class Name cannot exceed 100 characters")]
        [Display(Name = "Product Class Name (English)")]
        public string ProductClassName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم فئة المنتج (عربي)")]
        public string? ProductClassNameAr { get; set; }

        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [Display(Name = "Code")]
        public string? Code { get; set; }

        [StringLength(500, ErrorMessage = "Image path cannot exceed 500 characters")]
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(1000, ErrorMessage = "Arabic description cannot exceed 1000 characters")]
        [Display(Name = "الوصف (عربي)")]
        public string? DescriptionAr { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        // Navigation properties
        public virtual BusinessType? BusinessType { get; set; }
        public virtual ICollection<ProductType>? ProductTypes { get; set; }
    }
}