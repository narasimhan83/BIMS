using System;
using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    /// <summary>
    /// Defines a provider network (e.g., hospital / clinic network) under insurance management.
    /// </summary>
    public class ProviderNetwork
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Network Name is required")]
        [StringLength(150, ErrorMessage = "Network Name cannot exceed 150 characters")]
        [Display(Name = "Network Name")]
        public string NetworkName { get; set; } = string.Empty;

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