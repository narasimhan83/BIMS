using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class EngineCapacity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Display(Name = "Capacity (CC/Liters)")]
        public decimal Capacity { get; set; }

        [StringLength(50, ErrorMessage = "Display name cannot exceed 50 characters")]
        [Display(Name = "Display Name (English)")]
        public string? DisplayName { get; set; }

        [StringLength(50, ErrorMessage = "Arabic display name cannot exceed 50 characters")]
        [Display(Name = "الاسم المعروض (عربي)")]
        public string? DisplayNameAr { get; set; }

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