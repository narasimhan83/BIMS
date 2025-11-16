using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class SalesInvoice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Invoice number is required")]
        [Display(Name = "Invoice Number")]
        [StringLength(50, ErrorMessage = "Invoice number cannot exceed 50 characters")]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }

        [Required(ErrorMessage = "Sales Type is required")]
        [Display(Name = "Sales Type")]
        public int SalesTypeId { get; set; }

        [Display(Name = "Sales Type")]
        public virtual SalesType SalesType { get; set; }

        [Required(ErrorMessage = "Invoice date is required")]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [Display(Name = "Total Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Tax amount is required")]
        [Display(Name = "Tax Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Discount Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Required(ErrorMessage = "Net amount is required")]
        [Display(Name = "Net Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Net amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue, Cancelled

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
        public virtual ICollection<CreditNote> CreditNotes { get; set; } = new List<CreditNote>();
        public virtual ICollection<CustomerPayment> Payments { get; set; } = new List<CustomerPayment>();
    }
}