using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Make Name is required")]
        [StringLength(100, ErrorMessage = "Make Name cannot exceed 100 characters")]
        [Display(Name = "Make Name (English)")]
        public string MakeName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم الصانع (عربي)")]
        public string? MakeNameAr { get; set; }

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

        // Navigation property
        public virtual ICollection<VehicleModel>? VehicleModels { get; set; }
    }
}