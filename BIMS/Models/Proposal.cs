using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class Proposal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lead is required")]
        [Display(Name = "Lead")]
        public int LeadId { get; set; }

        [Display(Name = "Lead")]
        public virtual Lead Lead { get; set; }

        [Required(ErrorMessage = "Title (English) is required")]
        [Display(Name = "Title (English)")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string TitleEn { get; set; }

        [Required(ErrorMessage = "Title (Arabic) is required")]
        [Display(Name = "Title (Arabic)")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string TitleAr { get; set; }

        [Display(Name = "Description (English)")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string DescriptionEn { get; set; }

        [Display(Name = "Description (Arabic)")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string DescriptionAr { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Draft"; // Draft, Sent, Accepted, Rejected, Expired

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Sent Date")]
        public DateTime? SentDate { get; set; }

        [Display(Name = "Response Date")]
        public DateTime? ResponseDate { get; set; }

        [Display(Name = "Response Notes")]
        [StringLength(500, ErrorMessage = "Response notes cannot exceed 500 characters")]
        public string ResponseNotes { get; set; }

        [Required(ErrorMessage = "Valid until date is required")]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }
    }
}