# AEGIS IBMS - Customer Module Architecture

## Overview
Comprehensive Customer Management with personal details, conditional group selection, multiple KYC documents, and multiple vehicle information.

## Database Schema

### 1. Customer Table (Main Entity)
```sql
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY,
    
    -- Customer Classification
    CustomerTypeId INT NOT NULL,              -- FK to CustomerTypes
    CustomerGroupId INT NULL,                 -- FK to Groups (only if Type = Group)
    
    -- Contact Information
    CustomerName NVARCHAR(200) NOT NULL,
    CustomerNameAr NVARCHAR(200) NULL,
    InsuredName NVARCHAR(200) NULL,
    InsuredNameAr NVARCHAR(200) NULL,
    MobilePhone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(500) NULL,
    AddressAr NVARCHAR(500) NULL,
    
    -- Identification
    CPR_CR_Number NVARCHAR(50) NULL,          -- CPR for Individual, CR for Company
    VATNumber NVARCHAR(50) NULL,
    DateOfBirth DATE NULL,
    
    -- Status
    IsActive BIT NOT NULL DEFAULT 1,
    ConvertedToCustomer BIT NOT NULL DEFAULT 0,  -- Hidden from frontend
    
    -- Audit
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL,
    CreatedBy NVARCHAR(50) NULL,
    ModifiedBy NVARCHAR(50) NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_Customer_CustomerType FOREIGN KEY (CustomerTypeId) 
        REFERENCES CustomerTypes(Id),
    CONSTRAINT FK_Customer_Group FOREIGN KEY (CustomerGroupId) 
        REFERENCES Groups(Id)
)
```

### 2. CustomerDocument Table (One-to-Many)
```sql
CREATE TABLE CustomerDocuments (
    Id INT PRIMARY KEY IDENTITY,
    CustomerId INT NOT NULL,
    DocumentTypeId INT NOT NULL,
    
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT NULL,
    ContentType NVARCHAR(100) NULL,
    
    Description NVARCHAR(500) NULL,
    DescriptionAr NVARCHAR(500) NULL,
    
    UploadedDate DATETIME2 NOT NULL,
    UploadedBy NVARCHAR(50) NULL,
    
    CONSTRAINT FK_CustomerDocument_Customer FOREIGN KEY (CustomerId) 
        REFERENCES Customers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CustomerDocument_DocumentType FOREIGN KEY (DocumentTypeId) 
        REFERENCES DocumentTypes(Id)
)
```

### 3. CustomerVehicle Table (One-to-Many)
```sql
CREATE TABLE CustomerVehicles (
    Id INT PRIMARY KEY IDENTITY,
    CustomerId INT NOT NULL,
    
    -- Vehicle Information (FK to existing tables)
    VehicleMakeId INT NULL,
    VehicleModelId INT NULL,
    VehicleYearId INT NULL,
    EngineCapacityId INT NULL,
    
    -- Vehicle Details
    RegistrationNumber NVARCHAR(50) NULL,
    ChassisNumber NVARCHAR(50) NULL,
    
    Description NVARCHAR(500) NULL,
    DescriptionAr NVARCHAR(500) NULL,
    
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL,
    
    CONSTRAINT FK_CustomerVehicle_Customer FOREIGN KEY (CustomerId) 
        REFERENCES Customers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CustomerVehicle_Make FOREIGN KEY (VehicleMakeId) 
        REFERENCES VehicleMakes(Id),
    CONSTRAINT FK_CustomerVehicle_Model FOREIGN KEY (VehicleModelId) 
        REFERENCES VehicleModels(Id),
    CONSTRAINT FK_CustomerVehicle_Year FOREIGN KEY (VehicleYearId) 
        REFERENCES VehicleYears(Id),
    CONSTRAINT FK_CustomerVehicle_Capacity FOREIGN KEY (EngineCapacityId) 
        REFERENCES EngineCapacities(Id)
)
```

## C# Models

### Customer.cs
```csharp
public class Customer
{
    public int Id { get; set; }
    
    // Classification
    [Required] public int CustomerTypeId { get; set; }
    public int? CustomerGroupId { get; set; }
    
    // Personal Info (Bilingual)
    [Required] public string CustomerName { get; set; }
    public string? CustomerNameAr { get; set; }
    public string? InsuredName { get; set; }
    public string? InsuredNameAr { get; set; }
    
    // Contact
    public string? MobilePhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? AddressAr { get; set; }
    
    // Identification
    public string? CPR_CR_Number { get; set; }
    public string? VATNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    // Status
    public bool IsActive { get; set; }
    public bool ConvertedToCustomer { get; set; }  // Hidden
    
    // Navigation Properties
    public virtual CustomerType? CustomerType { get; set; }
    public virtual Group? CustomerGroup { get; set; }
    public virtual ICollection<CustomerDocument>? Documents { get; set; }
    public virtual ICollection<CustomerVehicle>? Vehicles { get; set; }
}
```

### CustomerDocument.cs
```csharp
public class CustomerDocument
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int DocumentTypeId { get; set; }
    
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long? FileSize { get; set; }
    public string? ContentType { get; set; }
    
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    
    public DateTime UploadedDate { get; set; }
    public string? UploadedBy { get; set; }
    
    public virtual Customer? Customer { get; set; }
    public virtual DocumentType? DocumentType { get; set; }
}
```

### CustomerVehicle.cs
```csharp
public class CustomerVehicle
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    
    public int? VehicleMakeId { get; set; }
    public int? VehicleModelId { get; set; }
    public int? VehicleYearId { get; set; }
    public int? EngineCapacityId { get; set; }
    
    public string? RegistrationNumber { get; set; }
    public string? ChassisNumber { get; set; }
    
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public virtual Customer? Customer { get; set; }
    public virtual VehicleMake? VehicleMake { get; set; }
    public virtual VehicleModel? VehicleModel { get; set; }
    public virtual VehicleYear? VehicleYear { get; set; }
    public virtual EngineCapacity? EngineCapacity { get; set; }
}
```

## Form Layout

### Customer Create/Edit Form

```
┌─────────────────────────────────────────────────┐
│  CUSTOMER INFORMATION                           │
├─────────────────────────────────────────────────┤
│  Customer Type: [Dropdown]                      │
│  Group: [Dropdown] * (shows only if Type=Group) │
│                                                  │
│  ┌──────────────┬──────────────┐               │
│  │ Name (EN)    │ Name (AR)    │               │
│  ├──────────────┼──────────────┤               │
│  │ Customer     │ العميل       │               │
│  │ Insured      │ المؤمن له    │               │
│  └──────────────┴──────────────┘               │
│                                                  │
│  Mobile: _________  Email: __________          │
│  CPR/CR: _________  VAT: ___________           │
│  Date of Birth: [Date Picker]                  │
│  Address (EN): _______________                 │
│  Address (AR): _______________ (RTL)           │
├─────────────────────────────────────────────────┤
│  KYC DOCUMENTS                                  │
├─────────────────────────────────────────────────┤
│  [+ Add Document]                               │
│                                                  │
│  Document 1:                                    │
│  Type: [Dropdown]  File: [Upload]  [Remove]   │
│                                                  │
│  Document 2:                                    │
│  Type: [Dropdown]  File: [Upload]  [Remove]   │
├─────────────────────────────────────────────────┤
│  VEHICLE INFORMATION                            │
├─────────────────────────────────────────────────┤
│  [+ Add Vehicle]                                │
│                                                  │
│  Vehicle 1:                                     │
│  Make: [Dropdown]  Model: [Dropdown]           │
│  Year: [Dropdown]  Capacity: [Dropdown]        │
│  Reg#: ______  Chassis#: ______  [Remove]      │
│                                                  │
│  Vehicle 2:                                     │
│  Make: [Dropdown]  Model: [Dropdown]           │
│  Year: [Dropdown]  Capacity: [Dropdown]        │
│  Reg#: ______  Chassis#: ______  [Remove]      │
├─────────────────────────────────────────────────┤
│  [Save Customer]  [Cancel]                      │
└─────────────────────────────────────────────────┘
```

## JavaScript Requirements

### Conditional Group Dropdown
```javascript
// Show/hide group dropdown based on customer type
$('#CustomerTypeId').change(function() {
    var selectedText = $(this).find('option:selected').text();
    if (selectedText.includes('Group') || selectedText.includes('مجموعة')) {
        $('#groupSection').show();
    } else {
        $('#groupSection').hide();
        $('#CustomerGroupId').val('');
    }
});
```

### Dynamic Document Addition
```javascript
// Add new document upload section
function addDocument() {
    var index = $('.document-row').length;
    var html = `
        <div class="document-row">
            <select name="Documents[${index}].DocumentTypeId"></select>
            <input type="file" name="Documents[${index}].File" />
            <button onclick="removeDocument(this)">Remove</button>
        </div>
    `;
    $('#documentsContainer').append(html);
}
```

### Dynamic Vehicle Addition
```javascript
// Add new vehicle section
function addVehicle() {
    var index = $('.vehicle-row').length;
    var html = `
        <div class="vehicle-row">
            <select name="Vehicles[${index}].VehicleMakeId"></select>
            <select name="Vehicles[${index}].VehicleModelId"></select>
            <select name="Vehicles[${index}].VehicleYearId"></select>
            <select name="Vehicles[${index}].EngineCapacityId"></select>
            <input name="Vehicles[${index}].RegistrationNumber" />
            <input name="Vehicles[${index}].ChassisNumber" />
            <button onclick="removeVehicle(this)">Remove</button>
        </div>
    `;
    $('#vehiclesContainer').append(html);
}
```

## File Upload Configuration

### appsettings.json
```json
{
  "FileUpload": {
    "MaxFileSize": 10485760,  // 10 MB
    "AllowedExtensions": [".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx"],
    "UploadPath": "uploads/customer-documents"
  }
}
```

### Program.cs
```csharp
// Configure file upload
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10MB
});
```

## Implementation Complexity

This is a **complex module** requiring:

### Models (3 new entities)
- Customer (with 15+ fields)
- CustomerDocument (for file uploads)
- CustomerVehicle (for vehicle details)

### Controllers
- CustomerController (extended with complex CRUD)
- File upload/download logic
- Dynamic form handling

### Views
- Customer Index (list with search/filter)
- Customer Create (complex form with dynamic sections)
- Customer Edit (load existing docs & vehicles)
- Customer Delete (cascade warning)
- Customer Details (view all info)

### JavaScript
- Conditional dropdown logic
- Dynamic document addition/removal
- Dynamic vehicle addition/removal
- Cascading dropdowns (Make → Model)
- File upload preview
- Form validation

### Features
- Multi-file upload
- File type validation
- File size validation
- Cascade deletion
- Complex foreign key relationships
- Conditional UI elements

## Estimated Effort

- **Models & Database**: 2 hours
- **Controllers & Logic**: 4 hours
- **Views & Forms**: 5 hours
- **JavaScript**: 3 hours
- **File Upload**: 2 hours
- **Testing**: 2 hours
- **Total**: ~18 hours

## Files to Create (~20 files)

**Models (3):**
- Customer.cs
- CustomerDocument.cs
- CustomerVehicle.cs

**ViewModels (potential 2):**
- CustomerViewModel.cs (for complex form)
-  DocumentUploadViewModel.cs

**Controllers:**
- Update CustomerController.cs (add Customers CRUD)

**Views (6+):**
- Customers.cshtml (Index)
- CreateCustomer.cshtml (Complex form)
- EditCustomer.cshtml (Complex form)
- DeleteCustomer.cshtml
- DetailsCustomer.cshtml (View all info)
- _DocumentRow.cshtml (Partial)
- _VehicleRow.cshtml (Partial)

**JavaScript (2):**
- customer-form.js (all dynamic logic)
- file-upload.js (upload handling)

**Migrations (1):**
- AddCustomerTables migration

---

**This is a major module. Should I proceed with full implementation?**

This will add significant functionality to your system:
- Complete customer management
- Document management with file uploads
- Vehicle tracking per customer
- Complex forms with dynamic sections