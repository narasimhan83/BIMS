using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class CustomerPayment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Payment reference is required")]
        [Display(Name = "Payment Reference")]
        [StringLength(50, ErrorMessage = "Payment reference cannot exceed 50 characters")]
        public string PaymentReference { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Sales Invoice")]
        public int? SalesInvoiceId { get; set; }

        [Display(Name = "Sales Invoice")]
        public virtual SalesInvoice SalesInvoice { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [Display(Name = "Payment Method")]
        [StringLength(30, ErrorMessage = "Payment method cannot exceed 30 characters")]
        public string PaymentMethod { get; set; } // Cash, Bank Transfer, Cheque, Credit Card

        [Display(Name = "Bank Reference")]
        [StringLength(100, ErrorMessage = "Bank reference cannot exceed 100 characters")]
        public string BankReference { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}