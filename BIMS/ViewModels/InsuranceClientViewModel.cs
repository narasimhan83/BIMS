using BIMS.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BIMS.ViewModels
{
    public class InsuranceClientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client Name is required")]
        [StringLength(100, ErrorMessage = "Client Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        public string? NameAr { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int CityId { get; set; }

        public bool IsActive { get; set; } = true;

        public List<BankDetailViewModel> BankDetails { get; set; } = new List<BankDetailViewModel>();
    }
}