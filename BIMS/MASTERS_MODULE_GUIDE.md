# AEGIS IBMS - Masters Module Documentation

## Overview
The Masters module provides centralized management of reference data used throughout the AEGIS Insurance Broker Management System. This includes Customer Types, Document Types, and Business Types.

## Features Implemented

### 1. Customer Types Management
Define and manage different categories of customers.

**Pre-loaded Data:**
- Individual
- Company  
- Group

**Fields:**
- Name (Required, max 100 characters)
- Description (Optional, max 500 characters)
- Active Status (Boolean)
- Created Date (Auto-generated)
- Modified Date (Auto-updated)

### 2. Document Types Management
Manage different types of documents used in the system.

**Fields:**
- Name (Required, max 100 characters)
- Code (Optional, max 50 characters)
- Description (Optional, max 500 characters)
- Active Status (Boolean)
- Created Date (Auto-generated)
- Modified Date (Auto-updated)

### 3. Business Types Management
Define different business categories and classifications.

**Fields:**
- Name (Required, max 100 characters)
- Code (Optional, max 50 characters)
- Description (Optional, max 500 characters)
- Active Status (Boolean)
- Created Date (Auto-generated)
- Modified Date (Auto-updated)

## Navigation

### Accessing Masters Menu
1. Login to AEGIS IBMS
2. Click on **"Masters"** dropdown in the navigation bar
3. Select from:
   - 👥 Customer Types
   - 📄 Document Types
   - 💼 Business Types

## CRUD Operations

### Create New Record
1. Navigate to the desired master type page
2. Click **"Add New [Type]"** button (teal button in top right)
3. Fill in the form:
   - Name (Required)
   - Code (Optional - for Document and Business Types)
   - Description (Optional)
   - Active checkbox (defaults to checked)
4. Click **"Create [Type]"** button
5. Success message will appear, and you'll be redirected to the list

### View Records
- All records are displayed in a sortable table
- Shows: Name, Code, Description, Status, Created Date, Actions
- Active records show green "Active" badge
- Inactive records show red "Inactive" badge
- Records are sorted by Created Date (newest first)

### Edit Record
1. Click the **yellow Edit** button (✏️) on the desired record
2. Modify the fields as needed
3. Click **"Save Changes"** button
4. Success message will appear

### Delete Record
1. Click the **red Delete** button (🗑️) on the desired record
2. Confirmation page appears with record details
3. Review the information
4. Click **"Yes, Delete"** to confirm
5. Success message will appear

## Database Tables

### CustomerTypes Table
```sql
CREATE TABLE CustomerTypes (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL
)
```

### DocumentTypes Table
```sql
CREATE TABLE DocumentTypes (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(50) NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL
)
```

### BusinessTypes Table
```sql
CREATE TABLE BusinessTypes (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(50) NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    ModifiedDate DATETIME2 NULL
)
```

## UI Components

### Table View Features
- Professional data table with header styling
- Hover effects on rows
- Color-coded status badges
- Icon-based action buttons
- Responsive design for mobile devices

### Form Features
- Clean, modern form layout
- Inline validation
- Clear error messages
- Auto-focus on first field
- Teal-themed submit buttons
- Cancel option on all forms

### Success Messages
- Green alert banner appears after successful operations
- Auto-dismisses after 5 seconds (can be configured)
- Positioned at top of page for visibility

## Validation Rules

### Customer Type
- Name: Required, max 100 characters
- Description: Optional, max 500 characters

### Document Type
- Name: Required, max 100 characters
- Code: Optional, max 50 characters
- Description: Optional, max 500 characters

### Business Type
- Name: Required, max 100 characters
- Code: Optional, max 50 characters
- Description: Optional, max 500 characters

## Security

- ✅ All Master pages require authentication ([Authorize] attribute)
- ✅ CSRF protection on all POST operations
- ✅ SQL injection prevention through EF Core
- ✅ XSS protection through Razor encoding

## Color Scheme

Consistent with AEGIS branding:
- **Headers**: Navy Dark (#0D4D4D)
- **Action Buttons**: Teal gradient
- **Edit Buttons**: Yellow/Orange (#ffc107)
- **Delete Buttons**: Red (#dc3545)
- **Success Badges**: Green (#28a745)

## File Structure

```
Controllers/
└── MastersController.cs (373 lines - all CRUD operations)

Models/
├── CustomerType.cs
├── DocumentType.cs
└── BusinessType.cs

Views/Masters/
├── CustomerTypes.cshtml (List view)
├── CreateCustomerType.cshtml
├── EditCustomerType.cshtml
├── DeleteCustomerType.cshtml
├── DocumentTypes.cshtml (List view)
├── CreateDocumentType.cshtml
├── EditDocumentType.cshtml
├── DeleteDocumentType.cshtml
├── BusinessTypes.cshtml (List view)
├── CreateBusinessType.cshtml
├── EditBusinessType.cshtml
└── DeleteBusinessType.cshtml

Data/
└── ApplicationDbContext.cs (Updated with DbSets and seed data)

Migrations/
└── 20251111125034_AddMasterTables.cs
```

## Testing Checklist

- [x] Customer Types - List view loads
- [x] Customer Types - Create new record (tested with "VIP Client")
- [x] Customer Types - Edit button works
- [x] Customer Types - Delete button works
- [x] Document Types - List view loads
- [x] Document Types - CRUD operations available
- [x] Business Types - List view loads  
- [x] Business Types - CRUD operations available
- [x] Navigation menu dropdown works
- [x] All pages require authentication
- [x] Seed data loaded (Individual, Company, Group)

## Future Enhancements

1. **Search & Filter**
   - Add search box to filter records by name
   - Filter by Active/Inactive status
   - Sort columns

2. **Bulk Operations**
   - Import from Excel
   - Export to Excel/CSV
   - Bulk activate/deactivate

3. **Audit Trail**
   - Track who created/modified records
   - View change history
   - Restore deleted records

4. **Advanced Features**
   - Custom fields per master type
   - Hierarchical categories
   - Tags and classifications

## Common Operations

### Adding Standard Customer Types
```
1. Individual - For personal insurance policies
2. Company - For corporate clients
3. Group - For group insurance schemes
4. VIP Client - High-value customers (already created)
```

### Common Document Types
```
- Policy Document (POL)
- Claim Form (CLM)
- ID Proof (ID)
- Address Proof (ADDR)
- Medical Certificate (MED)
```

### Common Business Types
```
- New Business (NEW)
- Renewal (REN)
- Endorsement (END)
- Cancellation (CAN)
```

## Support

For any issues with the Masters module:
1. Check database connectivity
2. Verify migrations are applied
3. Check browser console for JavaScript errors
4. Review server logs for exceptions

---

**Last Updated**: November 11, 2025
**Version**: 1.0
**Module**: Masters Management