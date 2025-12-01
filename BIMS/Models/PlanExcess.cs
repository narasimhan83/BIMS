using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    /// <summary>
    /// Links an Insurance Plan to an Excess Type and Value Band with a specific excess amount and unit.
    /// </summary>
    public class PlanExcess
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance Plan is required")]
        [Display(Name = "Insurance Plan")]
        public int InsurancePlanId { get; set; }

        [Required(ErrorMessage = "Excess Type is required")]
        [Display(Name = "Excess Type")]
        public int ExcessTypeId { get; set; }

        [Required(ErrorMessage = "Value Band is required")]
        [Display(Name = "Value Band")]
        public int ValueBandId { get; set; }

        [Required(ErrorMessage = "Excess Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Excess Amount")]
        public decimal ExcessAmount { get; set; }

        [Required(ErrorMessage = "Excess Unit is required")]
        [StringLength(20, ErrorMessage = "Excess Unit cannot exceed 20 characters")]
        [Display(Name = "Excess Unit")]
        public string ExcessUnit { get; set; } = string.Empty;

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
        public virtual ExcessType? ExcessType { get; set; }
        public virtual ValueBand? ValueBand { get; set; }
    }
}