using System;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    /// <summary>
    /// Links an Insurance Plan to a Provider Network.
    /// </summary>
    public class PlanNetwork
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Provider Network is required")]
        [Display(Name = "Provider Network")]
        public int ProviderNetworkId { get; set; }

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
        public virtual InsurancePlan? InsurancePlan { get; set; }
        public virtual ProviderNetwork? ProviderNetwork { get; set; }
    }
}