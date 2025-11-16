using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIMS.Models
{
    public class Customer
    {
        public int Id { get; set; }

        // Customer Classification
        [Required(ErrorMessage = "Customer Type is required")]
        [Display(Name = "Customer Type")]
        public int CustomerTypeId { get; set; }

        [Display(Name = "Customer Group")]
        public int? CustomerGroupId { get; set; }

        [Display(Name = "Agent")]
        public int? AgentId { get; set; }

        // Personal Information (Bilingual)
        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(200, ErrorMessage = "Customer Name cannot exceed 200 characters")]
        [Display(Name = "Customer Name (English)")]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "اسم العميل (عربي)")]
        public string? CustomerNameAr { get; set; }

        [StringLength(200)]
        [Display(Name = "Insured Name (English)")]
        public string? InsuredName { get; set; }

        [StringLength(200)]
        [Display(Name = "اسم المؤمن له (عربي)")]
        public string? InsuredNameAr { get; set; }

        // Contact Information
        [StringLength(20)]
        [Display(Name = "Mobile/Phone")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? MobilePhone { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Address (English)")]
        public string? Address { get; set; }

        [StringLength(500)]
        [Display(Name = "العنوان (عربي)")]
        public string? AddressAr { get; set; }

        // Identification
        [StringLength(50)]
        [Display(Name = "CPR/CR Number")]
        public string? CPR_CR_Number { get; set; }

        [StringLength(50)]
        [Display(Name = "VAT Number")]
        public string? VATNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "CPR Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? CPRExpiryDate { get; set; }

        [StringLength(20)]
        [Display(Name = "Passport No")]
        public string? PassportNo { get; set; }

        [Display(Name = "Passport Issue Date")]
        [DataType(DataType.Date)]
        public DateTime? PassportIssueDate { get; set; }

        [Display(Name = "Passport Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? PassportExpiryDate { get; set; }

        // Status Flags
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Hidden field - not shown in frontend
        public bool ConvertedToCustomer { get; set; } = false;

        // Audit Fields
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerTypeId")]
        public virtual CustomerType? CustomerType { get; set; }

        [ForeignKey("CustomerGroupId")]
        public virtual Group? CustomerGroup { get; set; }

        [ForeignKey("AgentId")]
        public virtual Agent? Agent { get; set; }

        public virtual ICollection<CustomerDocument>? Documents { get; set; }
        public virtual ICollection<CustomerVehicle>? Vehicles { get; set; }
    }
}