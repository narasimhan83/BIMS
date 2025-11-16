using System.ComponentModel.DataAnnotations;

namespace BIMS.ViewModels
{
    public class BankDetailViewModel
    {
        public int? Id { get; set; }

        public int BankId { get; set; }

        [StringLength(34, MinimumLength = 15, ErrorMessage = "IBAN must be between 15-34 characters")]
        public string IbanNumber { get; set; } = string.Empty;

        [StringLength(11, MinimumLength = 8, ErrorMessage = "Swift Code must be 8-11 characters")]
        public string SwiftCode { get; set; } = string.Empty;

        [StringLength(100)]
        public string Branch { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Custom validation method to validate only when data is provided
        public bool IsValid()
        {
            return BankId > 0 &&
                   !string.IsNullOrWhiteSpace(IbanNumber) &&
                   !string.IsNullOrWhiteSpace(SwiftCode) &&
                   !string.IsNullOrWhiteSpace(Branch);
        }
    }
}