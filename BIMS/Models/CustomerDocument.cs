using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class CustomerDocument
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Document Type is required")]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "File Name")]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "File Path")]
        public string FilePath { get; set; } = string.Empty;

        [Display(Name = "File Size (bytes)")]
        public long? FileSize { get; set; }

        [StringLength(100)]
        [Display(Name = "Content Type")]
        public string? ContentType { get; set; }

        [StringLength(500)]
        [Display(Name = "Description (English)")]
        public string? Description { get; set; }

        [StringLength(500)]
        [Display(Name = "الوصف (عربي)")]
        public string? DescriptionAr { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        [Display(Name = "Uploaded By")]
        public string? UploadedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType? DocumentType { get; set; }
    }
}