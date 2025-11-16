using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class CreditNote
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Credit note number is required")]
        [Display(Name = "Credit Note Number")]
        [StringLength(50, ErrorMessage = "Credit note number cannot exceed 50 characters")]
        public string CreditNoteNumber { get; set; }

        [Required(ErrorMessage = "Sales invoice is required")]
        [Display(Name = "Sales Invoice")]
        public int SalesInvoiceId { get; set; }

        [Display(Name = "Sales Invoice")]
        public virtual SalesInvoice SalesInvoice { get; set; }

        [Required(ErrorMessage = "Credit note date is required")]
        [Display(Name = "Credit Note Date")]
        public DateTime CreditNoteDate { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Reason (English) is required")]
        [Display(Name = "Reason (English)")]
        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters")]
        public string ReasonEn { get; set; }

        [Required(ErrorMessage = "Reason (Arabic) is required")]
        [Display(Name = "Reason (Arabic)")]
        [StringLength(200, ErrorMessage = "Reason cannot exceed 200 characters")]
        public string ReasonAr { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Draft"; // Draft, Approved, Applied

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}