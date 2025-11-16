using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class ProductType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Class is required")]
        [Display(Name = "Product Class")]
        public int ProductClassId { get; set; }

        [Required(ErrorMessage = "Product Type Name is required")]
        [StringLength(100, ErrorMessage = "Product Type Name cannot exceed 100 characters")]
        [Display(Name = "Product Type Name (English)")]
        public string ProductTypeName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم نوع المنتج (عربي)")]
        public string? ProductTypeNameAr { get; set; }

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
        public virtual ProductClass? ProductClass { get; set; }
    }
}