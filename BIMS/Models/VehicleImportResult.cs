namespace BIMS.Models
{
    public class VehicleImportResult
    {
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<VehicleImportError> Errors { get; set; } = new List<VehicleImportError>();
        public List<CustomerVehicle> ImportedVehicles { get; set; } = new List<CustomerVehicle>();
    }

    public class VehicleImportError
    {
        public int RowNumber { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class VehicleImportRow
    {
        public int RowNumber { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
    }
}