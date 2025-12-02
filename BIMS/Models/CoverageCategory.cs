using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Defines a coverage category for Motors, combining Product Class, Product Type, and Vehicle Category.
    /// </summary>
    public class CoverageCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Class is required")]
        [Display(Name = "Product Class")]
        public int ProductClassId { get; set; }

        [Required(ErrorMessage = "Product Type is required")]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Vehicle Category is required")]
        [Display(Name = "Vehicle Category")]
        public int VehicleCategoryId { get; set; }

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
        [ForeignKey(nameof(ProductClassId))]
        public virtual ProductClass? ProductClass { get; set; }

        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType? ProductType { get; set; }

        [ForeignKey(nameof(VehicleCategoryId))]
        public virtual VehicleCategory? VehicleCategory { get; set; }
    }
}