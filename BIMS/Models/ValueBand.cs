using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class ValueBand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value From is required")]
        [Display(Name = "Value From")]
        public decimal ValueFrom { get; set; }

        [Required(ErrorMessage = "Value To is required")]
        [Display(Name = "Value To")]
        public decimal ValueTo { get; set; }

        [StringLength(100)]
        [Display(Name = "Display Name (English)")]
        public string? DisplayName { get; set; }

        [StringLength(100)]
        [Display(Name = "الاسم المعروض (عربي)")]
        public string? DisplayNameAr { get; set; }

        [StringLength(500)]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(500)]
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