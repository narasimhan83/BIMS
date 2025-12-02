using System;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    /// <summary>
    /// Defines an additional cover under motor insurance (e.g. PA cover, accessories, etc.).
    /// </summary>
    public class AdditionalCover
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cover Code is required")]
        [StringLength(50, ErrorMessage = "Cover Code cannot exceed 50 characters")]
        [Display(Name = "Cover Code")]
        public string CoverCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cover Name is required")]
        [StringLength(150, ErrorMessage = "Cover Name cannot exceed 150 characters")]
        [Display(Name = "Cover Name (English)")]
        public string CoverName { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Arabic name cannot exceed 150 characters")]
        [Display(Name = "اسم التغطية (عربي)")]
        public string? CoverNameAr { get; set; }

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