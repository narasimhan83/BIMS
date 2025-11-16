using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class LineOfBusiness
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Client is required")]
        [Display(Name = "Insurance Client")]
        public int InsuranceClientId { get; set; }

        [Required(ErrorMessage = "Line of Business Name is required")]
        [StringLength(100, ErrorMessage = "Line of Business Name cannot exceed 100 characters")]
        [Display(Name = "Line of Business Name (English)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        [Display(Name = "اسم خط العمل (عربي)")]
        public string? NameAr { get; set; }

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

        // Navigation properties
        [ForeignKey("InsuranceClientId")]
        public virtual InsuranceClient? InsuranceClient { get; set; }
    }
}