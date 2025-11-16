# AEGIS IBMS - Arabic/RTL Support Architecture

## Overview
Adding bilingual support (English/Arabic) with RTL (Right-to-Left) layout capability to AEGIS IBMS for Saudi Arabian users.

## Implementation Strategy

### Approach 1: Database-Driven Bilingual Fields (Recommended)
Add Arabic name fields to existing tables, allowing both English and Arabic data entry.

**Advantages:**
- ✅ Simple to implement
- ✅ No complex localization framework needed
- ✅ Database stores both languages
- ✅ Easy to switch between languages
- ✅ Users can enter data in both languages

**Implementation Steps:**
1. Add Arabic fields to models (NameAr, DescriptionAr)
2. Update database schema
3. Modify forms to include Arabic input fields
4. Add language toggle in UI
5. Apply RTL CSS when Arabic is selected

### Approach 2: ASP.NET Core Localization
Use built-in localization with resource files.

**Advantages:**
- ✅ Industry standard approach
- ✅ Supports multiple languages easily
- ✅ Culture-specific formatting

**Disadvantages:**
- ⚠️ More complex setup
- ⚠️ Requires resource files management
- ⚠️ User data still in one language

## Recommended Solution: Hybrid Approach

Combine both approaches for the best user experience:
- **Database fields**: NameAr, DescriptionAr for user data
- **UI labels**: Resource files for interface translation
- **RTL support**: CSS with `dir="rtl"` attribute

## Database Schema Changes

### Updated Models

```csharp
public class CustomerType
{
    public int Id { get; set; }
    
    // English fields
    public string Name { get; set; }
    public string? Description { get; set; }
    
    // Arabic fields
    public string? NameAr { get; set; }
    public string? DescriptionAr { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
```

Same pattern for DocumentType and BusinessType.

### Migration

```csharp
migrationBuilder.AddColumn<string>(
    name: "NameAr",
    table: "CustomerTypes",
    type: "nvarchar(100)",
    maxLength: 100,
    nullable: true);

migrationBuilder.AddColumn<string>(
    name: "DescriptionAr", 
    table: "CustomerTypes",
    type: "nvarchar(500)",
    maxLength: 500,
    nullable: true);
```

## UI/UX Changes

### Language Switcher
Location: Navigation bar (next to username)

```html
<div class="language-switcher">
    <button onclick="setLanguage('en')" class="lang-btn" id="btn-en">
        <span>🇬🇧</span> English
    </button>
    <button onclick="setLanguage('ar')" class="lang-btn" id="btn-ar">
        <span>🇸🇦</span> العربية
    </button>
</div>
```

### RTL CSS Implementation

```css
/* RTL Support */
[dir="rtl"] {
    direction: rtl;
    text-align: right;
}

[dir="rtl"] .navbar-links {
    margin-right: 40px;
    margin-left: 0;
}

[dir="rtl"] .dropdown-menu {
    left: auto;
    right: 0;
}

[dir="rtl"] .form-group input,
[dir="rtl"] .form-group textarea {
    text-align: right;
}

[dir="rtl"] .btn-back::before {
    content: "→";
}

[dir="ltr"] .btn-back::before {
    content: "←";
}
```

### Form Updates

Forms will have both English and Arabic input fields side by side:

```html
<div class="bilingual-form">
    <div class="form-row">
        <div class="form-col">
            <label>Name (English)</label>
            <input asp-for="Name" placeholder="Enter name in English" />
        </div>
        <div class="form-col">
            <label>الاسم (عربي)</label>
            <input asp-for="NameAr" placeholder="أدخل الاسم بالعربية" dir="rtl" />
        </div>
    </div>
</div>
```

### Table Display Logic

Display the appropriate language based on user selection:

```csharp
@{
    var currentLang = Context.Session.GetString("Language") ?? "en";
    var displayName = currentLang == "ar" && !string.IsNullOrEmpty(item.NameAr) 
        ? item.NameAr 
        : item.Name;
}
<td><strong>@displayName</strong></td>
```

## Implementation Plan

### Step 1: Update Models
Add Arabic fields to:
- CustomerType (NameAr, DescriptionAr)
- DocumentType (NameAr, DescriptionAr)
- BusinessType (NameAr, DescriptionAr)

### Step 2: Create Migration
```bash
dotnet ef migrations add AddArabicFields
dotnet ef database update
```

### Step 3: Update Controllers
- Session management for language preference
- Pass language to views via ViewBag

### Step 4: Update Views

**For Each Master Type:**
1. Add bilingual form fields (English + Arabic side by side)
2. Update table display to show current language
3. Add language switcher to layout

### Step 5: Add RTL CSS
Create `rtl.css` file with:
- Direction changes
- Text alignment
- Margin/padding adjustments
- Icon/button positioning

### Step 6: Add JavaScript
Language switcher logic:
```javascript
function setLanguage(lang) {
    // Save preference
    sessionStorage.setItem('language', lang);
    // Update UI
    if (lang === 'ar') {
        document.documentElement.setAttribute('dir', 'rtl');
        document.body.classList.add('arabic');
    } else {
        document.documentElement.setAttribute('dir', 'ltr');
        document.body.classList.remove('arabic');
    }
    // Reload to apply changes
    location.reload();
}
```

## Arabic Font Support

### Recommended Fonts
1. **Tajawal** - Clean, modern Arabic font
2. **Cairo** - Professional, readable
3. **Noto Sans Arabic** - Google font, excellent support

### Font Integration
```css
@import url('https://fonts.googleapis.com/css2?family=Tajawal:wght@300;400;500;700&display=swap');

[dir="rtl"], [lang="ar"] {
    font-family: 'Tajawal', 'Segoe UI', Tahoma, sans-serif;
}
```

## Session Management

### Store Language Preference
```csharp
// In controller
HttpContext.Session.SetString("Language", "ar");

// Read in views
@{
    var currentLanguage = Context.Session.GetString("Language") ?? "en";
}
```

### Configure Session in Program.cs
```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

## UI Label Translations

### Common Labels

| English | Arabic |
|---------|--------|
| Dashboard | لوحة التحكم |
| Masters | الأساسيات |
| Customer Types | أنواع العملاء |
| Document Types | أنواع المستندات |
| Business Types | أنواع الأعمال |
| Create | إنشاء |
| Edit | تعديل |
| Delete | حذف |
| Save | حفظ |
| Cancel | إلغاء |
| Search | بحث |
| Active | نشط |
| Inactive | غير نشط |
| Name | الاسم |
| Description | الوصف |
| Code | الرمز |
| Created Date | تاريخ الإنشاء |
| Modified Date | تاريخ التعديل |
| Actions | الإجراءات |
| Back to List | العودة للقائمة |
| Welcome | أهلاً |
| Logout | تسجيل الخروج |
| Login | تسجيل الدخول |
| Register | تسجيل جديد |
| Username | اسم المستخدم |
| Password | كلمة المرور |
| Remember me | تذكرني |

## RTL Design Considerations

### Layout Adjustments Needed

1. **Navigation Menu**
   - Logo on right in RTL
   - Menu items flow right to left
   - Dropdown opens to left

2. **Forms**
   - Labels align right
   - Input text flows right to left
   - Buttons order reversed (Cancel on left, Submit on right)

3. **Tables**
   - Headers align right
   - Data flows right to left
   - Action buttons on left side

4. **Dashboard Cards**
   - Icons on right side
   - Text aligned right
   - Metrics flow right to left

5. **Arrows and Icons**
   - Back arrows point right (→)
   - Forward arrows point left (←)
   - Dropdown arrows adjust position

## File Structure for Bilingual Support

```
BIMS/
├── Resources/
│   ├── Views/
│   │   ├── Account/
│   │   │   ├── Login.en.resx
│   │   │   └── Login.ar.resx
│   │   ├── Masters/
│   │   │   ├── CustomerTypes.en.resx
│   │   │   └── CustomerTypes.ar.resx
│   │   └── Shared/
│   │       ├── _Layout.en.resx
│   │       └── _Layout.ar.resx
│   └── Controllers/
│       └── MastersController.ar.resx
├── wwwroot/css/
│   ├── site.css (LTR styles)
│   ├── site-rtl.css (RTL overrides)
│   └── arabic.css (Arabic-specific)
└── wwwroot/js/
    └── language-switcher.js
```

## Testing Checklist

### RTL Layout Testing
- [ ] Navigation menu flows correctly
- [ ] Dropdown menus open in correct direction
- [ ] Forms align properly
- [ ] Tables display correctly
- [ ] Buttons are in correct order
- [ ] Icons and arrows flip appropriately

### Arabic Text Testing
- [ ] Arabic font renders correctly
- [ ] Text flows right to left
- [ ] Mixed English/Arabic content handled well
- [ ] Form inputs accept Arabic text
- [ ] Database stores Arabic correctly (NVARCHAR)
- [ ] Search works with Arabic text

## Browser Compatibility

RTL support works in:
- ✅ Chrome 120+
- ✅ Firefox 115+
- ✅ Safari 16+
- ✅ Edge 120+
- ✅ Mobile browsers (iOS Safari, Chrome Mobile)

## Performance Considerations

- Minimal impact (<5% overhead)
- CSS loaded conditionally based on language
- Session-based language preference (fast)
- No page reload required for language switch (with AJAX)

## SEO & Accessibility

- `<html lang="ar">` or `lang="en"`
- `dir="rtl"` or `dir="ltr"` on html tag
- Arabic meta tags for SEO
- Screen reader support for both languages

## Migration Strategy

### Phase 1: Core Infrastructure
1. Add Arabic fields to database
2. Update models
3. Configure session management
4. Add basic RTL CSS

### Phase 2: UI Updates
1. Add language switcher
2. Update forms with bilingual fields
3. Update table displays
4. Add RTL styles

### Phase 3: Translation & Testing  
1. Translate all UI labels
2. Test RTL layout thoroughly
3. Test with real Arabic data
4. User acceptance testing

## Estimated Effort

- **Database changes**: 2 hours
- **UI updates**: 4 hours
- **RTL CSS**: 3 hours
- **Testing**: 2 hours
- **Total**: ~11 hours

## Priority Items

### High Priority (Must Have)
1. Arabic name fields in all master tables
2. RTL CSS support
3. Language switcher
4. Basic Arabic translations

### Medium Priority (Should Have)
1. Full UI translation
2. Help text in Arabic
3. Validation messages in Arabic

### Low Priority (Nice to Have)
1. Date formatting (Hijri calendar)
2. Number formatting (Arabic numerals)
3. Currency in Arabic

---

## Next Steps

Once approved, we'll switch to **Code Mode** to implement:
1. Add Arabic fields to all models
2. Create migration
3. Update views with bilingual forms
4. Add RTL CSS
5. Implement language switcher
6. Test thoroughly

This will make AEGIS IBMS fully accessible to Arabic-speaking users in Saudi Arabia! 🇸🇦