using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class CustomerType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Type Name is required")]
        [StringLength(100, ErrorMessage = "Type Name cannot exceed 100 characters")]
        [Display(Name = "Type Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "الاسم (عربي)")]
        public string? NameAr { get; set; }

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