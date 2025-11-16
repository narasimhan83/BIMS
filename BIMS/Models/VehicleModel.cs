using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vehicle Make is required")]
        [Display(Name = "Vehicle Make")]
        public int VehicleMakeId { get; set; }

        [Required(ErrorMessage = "Model Name is required")]
        [StringLength(100, ErrorMessage = "Model Name cannot exceed 100 characters")]
        [Display(Name = "Model Name (English)")]
        public string ModelName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم الموديل (عربي)")]
        public string? ModelNameAr { get; set; }

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
        [ForeignKey("VehicleMakeId")]
        public virtual VehicleMake? VehicleMake { get; set; }
    }
}