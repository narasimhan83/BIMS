# AEGIS IBMS - Vehicle Management Module Architecture

## Overview
Vehicle Management module under Masters for managing vehicle-related data with hierarchical relationships and full bilingual support.

## Entity Relationship

```
VehicleMake (1) ----< VehicleModel (Many)
   └─ Make: Toyota           └─ Models: Camry, Corolla, RAV4
   └─ Make: Honda            └─ Models: Accord, Civic, CR-V
   
VehicleYear (Independent)
   └─ Years: 2020, 2021, 2022, 2023, 2024
```

## Database Schema

### 1. VehicleMake Table
```sql
CREATE TABLE VehicleMakes (
    Id INT PRIMARY KEY IDENTITY,
    MakeName NVARCHAR(100) NOT NULL,
    MakeNameAr NVARCHAR(100) NULL,
    Description NVARCHAR(500) NULL,
    DescriptionAr NVARCHAR(500) NULL,
    Code NVARCHAR(50) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL,
    CreatedBy NVARCHAR(50) NULL,
    ModifiedBy NVARCHAR(50) NULL
)
```

### 2. VehicleModel Table (with FK to VehicleMake)
```sql
CREATE TABLE VehicleModels (
    Id INT PRIMARY KEY IDENTITY,
    ModelName NVARCHAR(100) NOT NULL,
    ModelNameAr NVARCHAR(100) NULL,
    VehicleMakeId INT NOT NULL,
    Description NVARCHAR(500) NULL,
    DescriptionAr NVARCHAR(500) NULL,
    Code NVARCHAR(50) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL,
    CreatedBy NVARCHAR(50) NULL,
    ModifiedBy NVARCHAR(50) NULL,
    FOREIGN KEY (VehicleMakeId) REFERENCES VehicleMakes(Id)
)
```

### 3. VehicleYear Table
```sql
CREATE TABLE VehicleYears (
    Id INT PRIMARY KEY IDENTITY,
    Year INT NOT NULL,
    YearDisplay NVARCHAR(50) NULL,      -- e.g., "2024 Model Year"
    YearDisplayAr NVARCHAR(50) NULL,    -- e.g., "موديل 2024"
    Description NVARCHAR(500) NULL,
    DescriptionAr NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL
)
```

## C# Models

### VehicleMake.cs
```csharp
public class VehicleMake
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    [Display(Name = "Make Name (English)")]
    public string MakeName { get; set; }
    
    [StringLength(100)]
    [Display(Name = "اسم الصانع (عربي)")]
    public string? MakeNameAr { get; set; }
    
    [StringLength(50)]
    public string? Code { get; set; }
    
    // Descriptions...
    
    // Navigation property
    public virtual ICollection<VehicleModel>? VehicleModels { get; set; }
}
```

### VehicleModel.cs
```csharp
public class VehicleModel
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Vehicle Make")]
    public int VehicleMakeId { get; set; }
    
    [Required]
    [StringLength(100)]
    [Display(Name = "Model Name (English)")]
    public string ModelName { get; set; }
    
    [StringLength(100)]
    [Display(Name = "اسم الموديل (عربي)")]
    public string? ModelNameAr { get; set; }
    
    // Navigation property
    [ForeignKey("VehicleMakeId")]
    public virtual VehicleMake? VehicleMake { get; set; }
}
```

## Navigation Menu Update

**Under Masters Dropdown:**
- ⚙️ Masters / الأساسيات
  - ... existing items ...
  - **🚗 Vehicle Management** / إدارة المركبات
    - 🏭 Vehicle Makes / صناع المركبات
    - 🚙 Vehicle Models / موديلات المركبات
    - 📅 Vehicle Years / سنوات الإنتاج

## Key Features

### Vehicle Make
- Bilingual name (English + Arabic)
- Code field (optional)
- Shows count of associated models
- Cascade options for delete (if has models)

### Vehicle Model
- **Dropdown selection of Make** (shows both English and Arabic names)
- Bilingual model name
- Displays parent make in table
- Foreign key relationship enforced

### Vehicle Year
- Year number (2020, 2021, etc.)
- Display name (bilingual): "2024 Model Year" / "موديل 2024"
- Description (optional)

## Form Examples

### Create Vehicle Model Form
```
┌─────────────────────────────────────┐
│  Select Vehicle Make: [Dropdown]    │
│  ┌──────────┬──────────┐            │
│  │ English  │ Arabic   │            │
│  ├──────────┼──────────┤            │
│  │ Camry    │ كامري    │            │
│  └──────────┴──────────┘            │
│  [Active] ☑                         │
│  [Create] [Cancel]                  │
└─────────────────────────────────────┘
```

## Implementation Priority

### High Priority (Phase 1)
1. Vehicle Make CRUD
2. Vehicle Model CRUD with Make dropdown
3. Vehicle Year CRUD

### Medium Priority (Phase 2)
1. Show model count in Make list
2. Prevent delete of Make if has models
3. Bulk import from Excel

### Low Priority (Phase 3)
1. Advanced filtering
2. Vehicle images
3. Specifications

## Navigation Flow

```
Dashboard → Masters → Vehicle Management
    ├── Vehicle Makes (List all makes)
    │   ├── Create Make
    │   ├── Edit Make
    │   └── Delete Make (warn if has models)
    │
    ├── Vehicle Models (List all models)
    │   ├── Create Model (select make from dropdown)
    │   ├── Edit Model (change make if needed)
    │   └── Delete Model
    │
    └── Vehicle Years (List all years)
        ├── Create Year
        ├── Edit Year
        └── Delete Year
```

## Sample Data

### Vehicle Makes
```
Toyota | تويوتا
Honda | هوندا
Nissan | نيسان
Hyundai | هيونداي
Kia | كيا
Ford | فورد
Chevrolet | شيفروليه
Mercedes-Benz | مرسيدس بنز
BMW | بي إم دبليو
Audi | أودي
```

### Vehicle Models (for Toyota)
```
Camry | كامري
Corolla | كورولا
RAV4 | راف4
Land Cruiser | لاند كروزر
Hilux | هايلوكس
Prado | برادو
Yaris | ياريس
Avalon | أفالون
```

### Vehicle Years
```
2020 Model Year | موديل 2020
2021 Model Year | موديل 2021
2022 Model Year | موديل 2022
2023 Model Year | موديل 2023
2024 Model Year | موديل 2024
2025 Model Year | موديل 2025
```

## Dropdown Behavior

### Make Dropdown in Model Form

**English Mode:**
```
Select Make... ▼
- Toyota
- Honda
- Nissan
```

**Arabic Mode:**
```
اختر الصانع... ▼
- تويوتا Toyota
- هوندا Honda
- نيسان Nissan
```

Shows both languages for clarity in dropdown even when in single language mode.

## Validation Rules

### Vehicle Make
- MakeName: Required, max 100 char
- MakeNameAr: Optional, max 100 char
- Code: Optional, max 50 char, unique
- Cannot delete if has associated models

### Vehicle Model
- VehicleMakeId: Required (must select a make)
- ModelName: Required, max 100 char
- ModelNameAr: Optional, max 100 char
- Code: Optional, max 50 char

### Vehicle Year
- Year: Required, numeric, range 1900-2100
- Unique year constraint
- YearDisplay: Auto-generated from Year

## Technical Implementation

### Files to Create (14 files)

**Models (3 files):**
- Models/VehicleMake.cs
- Models/VehicleModel.cs
- Models/VehicleYear.cs

**Controller (1 file):**
- Controllers/VehicleController.cs (with all CRUD for 3 entities)

**Views (12 files):**
- Views/Vehicle/VehicleMakes.cshtml
- Views/Vehicle/CreateVehicleMake.cshtml
- Views/Vehicle/EditVehicleMake.cshtml
- Views/Vehicle/DeleteVehicleMake.cshtml
- Views/Vehicle/VehicleModels.cshtml
- Views/Vehicle/CreateVehicleModel.cshtml
- Views/Vehicle/EditVehicleModel.cshtml
- Views/Vehicle/DeleteVehicleModel.cshtml
- Views/Vehicle/VehicleYears.cshtml
- Views/Vehicle/CreateVehicleYear.cshtml
- Views/Vehicle/EditVehicleYear.cshtml
- Views/Vehicle/DeleteVehicleYear.cshtml

### Migration
- Add VehicleMakes, VehicleModels, VehicleYears tables
- Add foreign key constraint
- Add indexes for performance

## Estimated Effort
- Models & Database: 1 hour
- Controllers: 2 hours
- Views: 3 hours
- Testing: 1 hour
- **Total: ~7 hours**

---

**Once approved, I'll switch to Code mode to implement all 3 vehicle entities with full CRUD and Arabic support!**