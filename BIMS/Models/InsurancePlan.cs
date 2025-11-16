using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class InsurancePlan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Client is required")]
        [Display(Name = "Insurance Client")]
        public int InsuranceClientId { get; set; }

        [Required(ErrorMessage = "Plan Name is required")]
        [StringLength(200, ErrorMessage = "Plan Name cannot exceed 200 characters")]
        [Display(Name = "Plan Name (English)")]
        public string PlanName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Arabic plan name cannot exceed 200 characters")]
        [Display(Name = "اسم الخطة (عربي)")]
        public string? PlanNameAr { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
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
        public virtual InsuranceClient? InsuranceClient { get; set; }
    }
}