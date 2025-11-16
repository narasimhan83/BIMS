# AEGIS IBMS - Customer Module Implementation Plan

## Status: Models Created, Ready for Implementation

### ✅ Completed So Far

1. **Customer Model** ✅ (109 lines)
   - 15+ fields with bilingual support
   - Foreign keys to CustomerType and Group
   - Navigation properties for Documents and Vehicles

2. **CustomerDocument Model** ✅ (64 lines)
   - File upload fields
   - Foreign keys to Customer and DocumentType
   - Upload tracking

3. **CustomerVehicle Model** ✅ (71 lines)
   - Foreign keys to all vehicle-related tables
   - Registration and chassis tracking
   - Bilingual description fields

4. **Database Context** ✅
   - Added Customers, CustomerDocuments, CustomerVehicles DbSets

5. **Migration** ⏳ (In Progress)
   - AddCustomerTables being created

---

## 🎯 Remaining Implementation Steps

### Phase 1: Database & Setup (2 hours)
- [ ] Apply AddCustomerTables migration
- [ ] Configure file upload settings in Program.cs
- [ ] Create wwwroot/uploads/customer-documents directory
- [ ] Add file upload configuration to appsettings.json

### Phase 2: Controller Logic (4 hours)
- [ ] Extend CustomerController with Customers CRUD
- [ ] Implement file upload/download methods
- [ ] Create dropdown population methods (Types, Groups, Makes, Models, etc.)
- [ ] Handle complex form submission with related data
- [ ] Implement cascading Model dropdown based on Make selection

### Phase 3: JavaScript Development (3 hours)
- [ ] customer-form.js - Conditional group dropdown
- [ ] customer-form.js - Dynamic document sections
- [ ] customer-form.js - Dynamic vehicle sections
- [ ] customer-form.js - Cascading dropdowns (Make → Model)
- [ ] file-upload.js - File validation and preview

### Phase 4: Views Creation (5 hours)
- [ ] Customers Index with search/filter
- [ ] CreateCustomer with complex multi-section form
- [ ] EditCustomer with existing documents & vehicles
- [ ] DetailsCustomer to view all information
- [ ] DeleteCustomer with cascade warning
- [ ] _DocumentRow partial view
- [ ] _VehicleRow partial view

### Phase 5: Navigation & Testing (2 hours)
- [ ] Add "Customers" menu item
- [ ] Test complete workflow
- [ ] Test file uploads
- [ ] Test dynamic sections
- [ ] Test Arabic/RTL with all features
- [ ] Test validation

**Total Remaining Effort: ~16 hours**

---

## 📋 Implementation Priority

### Critical (Must Have)
1. Basic Customer CRUD
2. Customer Type & Group selection (with conditional logic)
3. All personal/contact fields with bilingual support
4. Customer listing and details view

### High Priority (Should Have)
1. Single document upload per customer
2. Single vehicle per customer
3. File download functionality
4. Basic search/filter

###  Medium Priority (Nice to Have)
1. Multiple documents upload
2. Multiple vehicles per customer
3. Dynamic add/remove sections
4. Advanced search/filter
5. File preview

### Low Priority (Future Enhancement)
1. Document expiry tracking
2. Vehicle insurance tracking
3. Customer portal access
4. Email notifications
5. Document versioning

---

## 🔧 Technical Challenges

### File Upload Implementation
**Challenges:**
- File storage location
- File size limits
- Allowed file types
- Unique file naming
- Security (prevent malicious uploads)

**Solution:**
```csharp
// In CustomerController
[HttpPost]
public async Task<IActionResult> UploadDocument(int customerId, 
    IFormFile file, int documentTypeId)
{
    if (file != null && file.Length > 0)
    {
        // Validate file type
        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type");
        
        // Validate size (10MB max)
        if (file.Length > 10485760)
            return BadRequest("File too large");
        
        // Generate unique filename
        var fileName = $"{customerId}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine("wwwroot/uploads/customer-documents", fileName);
        
        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        // Save to database
        var doc = new CustomerDocument
        {
            CustomerId = customerId,
            DocumentTypeId = documentTypeId,
            FileName = file.FileName,
            FilePath = filePath,
            FileSize = file.Length,
            ContentType = file.ContentType,
            UploadedDate = DateTime.UtcNow,
            UploadedBy = User.Identity.Name
        };
        _context.CustomerDocuments.Add(doc);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    return BadRequest("No file uploaded");
}
```

### Dynamic Form Sections
**Challenges:**
- Managing array of documents in form
- Managing array of vehicles in form
- Proper model binding
- Validation of dynamic sections

**Solution:**
Use indexed collection binding:
```html
<!-- Document 1 -->
<input name="Documents[0].DocumentTypeId" />
<input name="Documents[0].File" type="file" />

<!-- Document 2 -->
<input name="Documents[1].DocumentTypeId" />
<input name="Documents[1].File" type="file" />
```

### Conditional Group Dropdown
**Challenges:**
- Show/hide based on customer type
- Load groups from database
- Handle postback validation

**Solution:**
```javascript
$('#CustomerTypeId').change(function() {
    var typeId = $(this).val();
    // Get type name to check if "Group"
    $.get('/Customer/IsGroupType?typeId=' + typeId, function(isGroup) {
        if (isGroup) {
            $('#groupSection').slideDown();
        } else {
            $('#groupSection').slideUp();
            $('#CustomerGroupId').val('');
        }
    });
});
```

---

## 📝 Sample Customer Data

### Individual Customer
```
Type: Individual
Name: Ahmed Mohammed / أحمد محمد
Mobile: +966501234567
CPR: 850101-1234
DOB: 01/01/1985
Email: ahmed@email.com
Address: Riyadh, Saudi Arabia / الرياض، المملكة

Documents:
- National ID (PDF)
- Residence Permit (PDF)

Vehicles:
- Toyota Camry 2023, 1600 CC
  Reg: ABC-1234, Chassis: JT2BK18E0X0123456
```

### Company Customer
```
Type: Company
Name: ABC Trading Co. / شركة ABC للتجارة
CR Number: 1234567890
VAT: 123456789012345
Email: info@abc.com
Address: King Fahd Road, Riyadh

Documents:
- Commercial Registration (PDF)
- VAT Certificate (PDF)
- Authorization Letter (PDF)

Vehicles:
- Multiple vehicles (fleet)
```

### Group Customer
```
Type: Group
Group: Medical Professionals Group
Name: Doctors Association / جمعية الأطباء
Members: Multiple individuals

Documents:
- Group Agreement (PDF)
- Member List (Excel)

Vehicles:
- Personal vehicles of group members
```

---

## 🎨 UI/UX Design

### Customer Form Sections

**Section 1: Customer Classification**
- Customer Type (dropdown)
- Group (conditional dropdown)

**Section 2: Personal Information**
- Customer Name (bilingual)
- Insured Name (bilingual)
- Date of Birth
- CPR/CR Number

**Section 3: Contact Information**
- Mobile/Phone
- Email
- Address (bilingual)
- VAT Number

**Section 4: KYC Documents**
- [+ Add Document] button
- Dynamic document rows
- Each row: Type dropdown + File upload + Remove button

**Section 5: Vehicle Information**
- [+ Add Vehicle] button
- Dynamic vehicle rows
- Each row: Make/Model/Year/Capacity dropdowns + Reg# + Chassis# + Remove

**Section 6: Actions**
- [Save Customer] [Cancel]

---

## 🚀 Quick Start Guide (When Ready)

### 1. Apply Migration
```bash
dotnet ef database update
```

### 2. Configure File Upload
Add to appsettings.json:
```json
"FileUpload": {
  "MaxSizeMB": 10,
  "AllowedTypes": [".pdf", ".jpg", ".png", ".docx"],
  "BasePath": "wwwroot/uploads/customer-documents"
}
```

### 3. Create Upload Directory
```bash
mkdir wwwroot\uploads
mkdir wwwroot\uploads\customer-documents
```

### 4. Run Application
```bash
dotnet run
```

### 5. Test
1. Navigate: Customer Management → Customers
2. Click "Add New Customer"
3. Select Type "Group" → Group dropdown appears
4. Fill all fields
5. Upload documents
6. Add vehicles
7. Save and verify

---

## 📊 Estimated Database Growth

With 100 customers:
- Customers table: 100 rows
- CustomerDocuments: ~300 rows (avg 3 docs/customer)
- CustomerVehicles: ~150 rows (avg 1.5 vehicles/customer)

**Total: ~550 new rows** for customer data

---

## 🔐 Security Considerations

1. **File Upload Security**
   - Validate file extensions
   - Scan for malware (future)
   - Limit file size
   - Use unique generated filenames
   - Store outside wwwroot for production

2. **Data Privacy**
   - Encrypt sensitive fields (CPR, CR numbers)
   - Access control for documents
   - Audit trail for all changes
   - GDPR compliance for customer data

3. **Validation**
   - Server-side validation (cannot trust client)
   - SQL injection prevention (EF Core handles)
   - XSS prevention (Razor handles)
   - CSRF tokens on forms

---

## 📈 Next Steps

**Immediate:**
1. Wait for migration to complete
2. Apply migration to database
3. Start implementing CustomerController methods
4. Create basic Customer Index view
5. Create CreateCustomer form (start simple, add complexity)

**Then:**
1. Add file upload functionality
2. Add JavaScript for dynamic sections
3. Test thoroughly
4. Add advanced features

---

**This Customer module will be the centerpiece of your IBMS system!**

**It ties together all the masters data (Types, Groups, Documents, Vehicles) into a complete customer management solution.**