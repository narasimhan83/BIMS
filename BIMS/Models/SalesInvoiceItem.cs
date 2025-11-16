using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class SalesInvoiceItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sales invoice is required")]
        [Display(Name = "Sales Invoice")]
        public int SalesInvoiceId { get; set; }

        [Display(Name = "Sales Invoice")]
        public virtual SalesInvoice SalesInvoice { get; set; }

        [Required(ErrorMessage = "Description (English) is required")]
        [Display(Name = "Description (English)")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessage = "Description (Arabic) is required")]
        [Display(Name = "Description (Arabic)")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string DescriptionAr { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Display(Name = "Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Total price is required")]
        [Display(Name = "Total Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be positive")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}