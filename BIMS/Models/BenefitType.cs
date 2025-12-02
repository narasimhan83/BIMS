using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Defines a motor insurance benefit type, linked to a specific vehicle category.
    /// </summary>
    public class BenefitType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vehicle Category is required")]
        [Display(Name = "Vehicle Category")]
        public int VehicleCategoryId { get; set; }

        [Required(ErrorMessage = "Benefit Type Name is required")]
        [StringLength(150, ErrorMessage = "Benefit Type Name cannot exceed 150 characters")]
        [Display(Name = "Benefit Type Name (English)")]
        public string BenefitTypeName { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Arabic name cannot exceed 150 characters")]
        [Display(Name = "نوع المنفعة (عربي)")]
        public string? BenefitTypeNameAr { get; set; }

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

        // Navigation properties
        [ForeignKey(nameof(VehicleCategoryId))]
        public virtual VehicleCategory? VehicleCategory { get; set; }
    }
}