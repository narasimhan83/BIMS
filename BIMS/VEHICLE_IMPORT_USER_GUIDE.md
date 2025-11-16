# Vehicle Import from Excel - User Guide

## Overview
The Vehicle Import feature allows you to bulk import vehicle data from Excel files when editing customer records. This is particularly useful for company-type customers with multiple vehicles.

---

## How to Use

### Step 1: Navigate to Edit Customer Page
1. Go to **Customers** list
2. Click **Edit** on any customer
3. Scroll down to the **"Add New Vehicles"** section

### Step 2: Download Excel Template
1. Click the **"Import from Excel"** button (green button with 📊 icon)
2. In the modal, click **"Download Template"** button
3. An Excel file named `Vehicle_Import_Template_YYYYMMDD.xlsx` will be downloaded

### Step 3: Fill Excel Template
Open the downloaded template and fill in vehicle data:

| Column | Required | Description | Example |
|--------|----------|-------------|---------|
| **Make** | ✅ Yes | Vehicle manufacturer name | Toyota |
| **Model** | ✅ Yes | Vehicle model name | Camry |
| **Year** | ✅ Yes | Manufacturing year | 2023 |
| **Engine Capacity** | ❌ No | Engine size | 2.5L or 2500 CC |
| **Registration Number** | ❌ No | License plate number | ABC-1234 |
| **Chassis Number** | ❌ No | VIN/Chassis number | JT123456789012345 |

**Important Notes:**
- Make, Model, and Year are **required**
- Use names that exist in your system (case-insensitive)
- Both English and Arabic names are supported
- Registration and Chassis numbers must be unique per customer
- Delete the sample data row before importing

### Step 4: Upload and Import
1. Click **"Select Excel File"** button
2. Choose your filled Excel file
3. Click **"Import"** button
4. Wait for processing (progress indicator will show)

### Step 5: Review Results
After import completes, you'll see:
- ✅ **Success Count**: Number of vehicles imported
- ❌ **Error Count**: Number of rows with errors
- 📊 **Total Count**: Total rows processed

If there are errors:
- A detailed error table will show which rows failed and why
- Fix the errors in Excel and import again
- Successfully imported vehicles are already saved

### Step 6: Refresh Page
Click **"Refresh Page"** to see the newly imported vehicles in the list.

---

## Supported Data Formats

### Make Names
The system will match vehicle makes by name (case-insensitive):
- English names: `Toyota`, `Nissan`, `Ford`, etc.
- Arabic names: `تويوتا`, `نيسان`, etc.

### Model Names
Models are matched within their respective makes:
- English: `Camry`, `Altima`, `Mustang`
- Arabic: `كامري`, `التيما`, etc.

### Year Format
- Use 4-digit year: `2023`, `2024`
- Must exist in your system's Vehicle Years

### Engine Capacity Formats
The system accepts multiple formats:
- Liters: `2.5L`, `3.0L`
- CC: `2500 CC`, `3000 CC`
- Numbers: `2500`, `3000`

---

## Error Messages

### Common Errors and Solutions

| Error | Cause | Solution |
|-------|-------|----------|
| "Make '{name}' not found" | Make doesn't exist in system | Use exact name from system or add make first |
| "Model '{name}' not found for Make '{make}'" | Model doesn't exist for that make | Check model spelling or add model first |
| "Year '{year}' not found" | Year doesn't exist in system | Add year to Vehicle Years master data |
| "Registration number already exists" | Duplicate reg number for customer | Use unique registration number |
| "Chassis number already exists" | Duplicate chassis for customer | Use unique chassis number |
| "Make is required" | Empty make column | Fill in make name |
| "Model is required" | Empty model column | Fill in model name |
| "Year is required" | Empty year column | Fill in year |
| "Invalid year format" | Non-numeric year | Use 4-digit number like 2023 |
| "File is empty" | No data rows in Excel | Add at least one vehicle |
| "Invalid file type" | Not an Excel file | Use .xlsx or .xls file |
| "File size too large" | File over 5MB | Reduce file size or split into multiple files |

---

## Best Practices

### 1. Verify Master Data First
Before importing, ensure your system has:
- ✅ All vehicle makes configured
- ✅ All models for each make
- ✅ Years you need (current and past)
- ✅ Engine capacities (optional)

### 2. Data Preparation
- Use consistent naming (don't mix English/Arabic in same column)
- Double-check spelling of make/model names
- Remove any blank rows
- Keep file size under 5MB (approximately 1000-2000 rows)

### 3. Import Strategy
For large datasets:
- **Option A**: Import in batches of 500-1000 vehicles
- **Option B**: Import all at once (if under 5MB)
- Fix errors and re-import failed rows

### 4. Error Handling
- Review all errors in the results modal
- Copy error details if needed
- Fix issues in Excel file
- Re-import only the fixed rows (remove successful ones)

---

## Example Import Scenarios

### Scenario 1: Small Fleet (10-50 vehicles)
1. Download template
2. Fill all 50 vehicles in one file
3. Import in single operation
4. Review results and fix any errors

### Scenario 2: Medium Fleet (50-500 vehicles)
1. Download template
2. Fill vehicles in Excel (or copy from existing data)
3. Import all at once
4. If errors occur, fix and re-import failed rows only

### Scenario 3: Large Fleet (500+ vehicles)
1. Split data into multiple batches (500 per file)
2. Import first batch
3. Review results
4. Import remaining batches
5. Consolidate error reports

---

## Troubleshooting

### Import Button Not Working
- **Check**: Browser JavaScript is enabled
- **Check**: You're on Edit Customer page (not Create)
- **Solution**: Refresh page and try again

### Template Download Fails
- **Check**: Pop-up blocker is not blocking download
- **Solution**: Allow downloads from the site

### File Upload Fails
- **Check**: File size is under 5MB
- **Check**: File extension is .xlsx or .xls
- **Check**: File is not corrupted
- **Solution**: Re-save file in Excel and try again

### All Rows Showing Errors
- **Check**: Column headers match template exactly
- **Check**: Make/Model names are spelled correctly
- **Check**: You're using names that exist in system
- **Solution**: Download fresh template and try again

### Partial Import Success
- **Result**: Valid rows are imported, invalid rows are reported
- **Action**: Fix invalid rows and re-import only those rows
- **Note**: Valid rows are already saved and won't be duplicated

---

## FAQ

### Q: Can I update existing vehicles via import?
**A:** No, the current version only supports adding new vehicles. To update, delete old vehicle and import new one.

### Q: What happens to valid rows if some rows have errors?
**A:** Valid rows are imported successfully. Only rows with errors are skipped.

### Q: Can I import vehicles during customer creation?
**A:** No, import is only available on Edit Customer page (after customer exists).

### Q: Is there a limit on number of vehicles?
**A:** No hard limit, but file size must be under 5MB (roughly 1000-2000 vehicles).

### Q: Can I use Arabic names only?
**A:** Yes, you can use either English or Arabic names, system will match both.

### Q: What if Make/Model doesn't exist?
**A:** You must first add it to Masters > Vehicle Make/Model, then import.

### Q: Can I import empty optional fields?
**A:** Yes, Engine Capacity, Registration Number, and Chassis Number are optional.

### Q: Will it detect duplicates?
**A:** Yes, it checks for duplicate Registration and Chassis numbers within the same customer.

---

## Technical Specifications

- **Maximum File Size**: 5 MB
- **Supported Formats**: .xlsx, .xls
- **Maximum Rows**: ~2000 (depending on data)
- **Processing Time**: ~1-5 seconds per 100 rows
- **Character Encoding**: UTF-8 (supports Arabic)
- **Datetime Format**: Server timezone (UTC)
- **Validation**: Real-time during import
- **Transaction**: Atomic (all or partial success)

---

## Future Enhancements (Roadmap)

- [ ] Bulk update of existing vehicles
- [ ] Delete flag in import (mark for deletion)
- [ ] Async processing for very large files
- [ ] Import history tracking
- [ ] Smart validation suggestions (did you mean...)
- [ ] Preview before import
- [ ] Error report Excel download
- [ ] Template customization

---

## Support

If you encounter issues not covered in this guide:
1. Check browser console for error messages
2. Verify all master data exists in system
3. Try with smaller file first
4. Contact system administrator for help

---

## Quick Reference Card

```
STEPS TO IMPORT:
1. Edit Customer → Scroll to Vehicles
2. Click "Import from Excel" (green button)
3. Download Template
4. Fill: Make, Model, Year (required) + others (optional)
5. Upload filled file
6. Click Import
7. Review results
8. Fix errors if any
9. Refresh page

REQUIRED FIELDS:
✅ Make
✅ Model  
✅ Year

OPTIONAL FIELDS:
❌ Engine Capacity
❌ Registration Number
❌ Chassis Number

FILE LIMITS:
📁 Max Size: 5 MB
📄 Format: .xlsx or .xls
📊 Rows: ~2000 max
```

---

*Last Updated: 2025-11-12*
*Version: 1.0*