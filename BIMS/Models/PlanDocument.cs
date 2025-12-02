using System;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    /// <summary>
    /// Represents a document attached to an insurance plan.
    /// </summary>
    public class PlanDocument
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Document URL is required")]
        [StringLength(500, ErrorMessage = "Document URL cannot exceed 500 characters")]
        [Display(Name = "Document URL")]
        public string DocumentUrl { get; set; } = string.Empty;

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

        // Navigation
        public virtual InsurancePlan? InsurancePlan { get; set; }
    }
}