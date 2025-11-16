using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class Lead
    {
        public int Id { get; set; }
    
        [Required(ErrorMessage = "Customer is required")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
    
        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }
    
        [Required(ErrorMessage = "English name is required")]
        [Display(Name = "Name (English)")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "Arabic name is required")]
        [Display(Name = "Name (Arabic)")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Display(Name = "Phone")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Display(Name = "Company (English)")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        public string CompanyEn { get; set; }

        [Display(Name = "Company (Arabic)")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        public string CompanyAr { get; set; }

        [Required(ErrorMessage = "Source is required")]
        [Display(Name = "Lead Source")]
        [StringLength(50, ErrorMessage = "Source cannot exceed 50 characters")]
        public string Source { get; set; } // Website, Referral, Cold Call, etc.

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "New"; // New, Contacted, Qualified, Proposal, Won, Lost

        [Required(ErrorMessage = "Potential value is required")]
        [Display(Name = "Potential Value")]
        [Range(0, double.MaxValue, ErrorMessage = "Potential value must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PotentialValue { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Last Contact Date")]
        public DateTime? LastContactDate { get; set; }

        [Display(Name = "Assigned To")]
        [StringLength(100, ErrorMessage = "Assigned to cannot exceed 100 characters")]
        public string AssignedTo { get; set; } // User ID or Name

        // Navigation property
        public virtual ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    }
}