using System.ComponentModel.DataAnnotations;

namespace BIMS.Models
{
    public class AgentBankDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Agent is required")]
        [Display(Name = "Agent")]
        public int AgentId { get; set; }

        [Required(ErrorMessage = "Bank is required")]
        [Display(Name = "Bank")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "IBAN Number is required")]
        [StringLength(34, MinimumLength = 15, ErrorMessage = "IBAN Number must be between 15-34 characters")]
        [Display(Name = "IBAN Number")]
        [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{11,30}$", ErrorMessage = "Please enter a valid IBAN number")]
        public string IbanNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Swift Code is required")]
        [StringLength(11, MinimumLength = 8, ErrorMessage = "Swift Code must be 8 or 11 characters")]
        [Display(Name = "Swift Code")]
        [RegularExpression(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", ErrorMessage = "Please enter a valid Swift Code")]
        public string SwiftCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Branch is required")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
        [Display(Name = "Branch")]
        public string Branch { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Is Primary")]
        public bool IsPrimary { get; set; } = false;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public virtual Agent? Agent { get; set; }
        public virtual Bank? Bank { get; set; }
    }
}