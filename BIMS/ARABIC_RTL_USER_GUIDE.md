# AEGIS IBMS - Arabic/RTL Support User Guide

## 🇸🇦 مرحباً بك في نظام إيجيس للوساطة التأمينية

## Overview / نظرة عامة

AEGIS IBMS now fully supports **Arabic language** with **Right-to-Left (RTL)** layout for Saudi Arabian users. You can:
- Switch between English and Arabic instantly
- Enter data in both languages simultaneously
- View interface in your preferred language
- Work with professional Arabic typography

## Language Switching / تبديل اللغة

### How to Switch Language

**In Navigation Bar:**
1. Look for the language switcher (next to your username)
2. Click **🇬🇧 English** for English interface
3. Click **🇸🇦 العربية** for Arabic interface
4. Page will reload with the selected language

**Language Preference:**
- Your selection is saved automatically
- Preference persists across sessions
- No need to change language each time you login

## Features / الميزات

### 1. Bilingual Data Entry / إدخال البيانات بلغتين

All Master forms now have **side-by-side fields** for both languages:

**Customer Types Form:**
- English Name Field | حقل الاسم بالعربية
- English Description | الوصف بالعربية

**Document Types Form:**
- English Name Field | حقل الاسم بالعربية
- Code Field (shared)
- English Description | الوصف بالعربية

**Business Types Form:**
- English Name Field | حقل الاسم بالعربية
- Code Field (shared)
- English Description | الوصف بالعربية

### 2. Automatic RTL Layout / تخطيط من اليمين لليسار

When Arabic is selected:
- ✅ Text flows right-to-left
- ✅ Navigation menu aligns to the right
- ✅ Tables display from right-to-left
- ✅ Buttons reorder appropriately
- ✅ Dropdown menus open correctly

### 3. Arabic Typography / الطباعة العربية

**Fonts Used:**
- **Tajawal** - Primary Arabic font
- **Cairo** - Fallback font
- Clean, professional appearance

**Text Rendering:**
- Crystal clear Arabic text
- Proper letter connections
- Professional spacing

## User Interface Translation / ترجمة واجهة المستخدم

### Navigation / التنقل

| English | Arabic |
|---------|--------|
| Dashboard | لوحة التحكم |
| Masters | الأساسيات |
| Customer Types | أنواع العملاء |
| Document Types | أنواع المستندات |
| Business Types | أنواع الأعمال |
| Welcome | أهلاً |
| Logout | تسجيل خروج |

### Table Headers / رؤوس الجداول

| English | Arabic |
|---------|--------|
| Type Name | اسم النوع |
| Document Type Name | اسم نوع المستند |
| Business Type Name | اسم نوع العمل |
| Code | الرمز |
| Description | الوصف |
| Status | الحالة |
| Active | نشط |
| Inactive | غير نشط |
| Created Date | تاريخ الإنشاء |
| Modified Date | تاريخ التعديل |
| Actions | الإجراءات |

### Buttons / الأزرار

| English | Arabic |
|---------|--------|
| Add New Customer Type | إضافة نوع عميل جديد |
| Add New Document Type | إضافة نوع مستند جديد |
| Add New Business Type | إضافة نوع عمل جديد |
| Create | إنشاء |
| Save Changes | حفظ التغييرات |
| Delete | حذف |
| Cancel | إلغاء |
| Back to List | العودة للقائمة |
| Yes, Delete | نعم، احذف |

## Data Display Logic / منطق عرض البيانات

### How Data is Displayed / كيفية عرض البيانات

**In English Mode:**
- Shows English name (Name field)
- Shows English description (Description field)
- Falls back to English if Arabic is empty

**In Arabic Mode:**
- Shows Arabic name (NameAr field) if available
- Shows Arabic description (DescriptionAr field) if available
- Falls back to English if Arabic is not provided

**Example:**
```
English Mode: "VIP Client" | "High-value customers..."
Arabic Mode: "عميل VIP" | "عملاء ذوي قيمة عالية..."
```

## Database Schema / مخطط قاعدة البيانات

### Updated Tables / الجداول المحدثة

**CustomerTypes:**
- `Name` NVARCHAR(100) - English name
- `NameAr` NVARCHAR(100) - Arabic name
- `Description` NVARCHAR(500) - English description
- `DescriptionAr` NVARCHAR(500) - Arabic description

**DocumentTypes:**
- `Name` NVARCHAR(100) - English name
- `NameAr` NVARCHAR(100) - Arabic name
- `Code` NVARCHAR(50) - Code (shared)
- `Description` NVARCHAR(500) - English description
- `DescriptionAr` NVARCHAR(500) - Arabic description

**BusinessTypes:**
- `Name` NVARCHAR(100) - English name
- `NameAr` NVARCHAR(100) - Arabic name
- `Code` NVARCHAR(50) - Code (shared)
- `Description` NVARCHAR(500) - English description
- `DescriptionAr` NVARCHAR(500) - Arabic description

## Best Practices / أفضل الممارسات

### Data Entry Recommendations

1. **Always Enter English Name** (Required)
   - English name is mandatory
   - Used as fallback in Arabic mode

2. **Add Arabic Name** (Optional but Recommended)
   - Helps Arabic users understand better
   - Professional appearance in Arabic mode

3. **Code Field** (Use English/Numbers)
   - Keep codes in English for consistency
   - Examples: POL, CLM, ID, NEW, REN

4. **Descriptions** (Enter Both if Possible)
   - Helps bilingual teams
   - Better for reports and documentation

### Example Data Entry

**Customer Type:**
```
Name (English): VIP Client
الاسم (عربي): عميل VIP
Description: High-value customers with premium services
الوصف: عملاء ذوي قيمة عالية مع خدمات مميزة
```

**Document Type:**
```
Name (English): Policy Document
الاسم (عربي): وثيقة التأمين
Code: POL
Description: Main insurance policy document
الوصف: وثيقة التأمين الرئيسية
```

## Technical Details / التفاصيل التقنية

### Files Added for Arabic Support

**CSS Files:**
- `wwwroot/css/rtl.css` (286 lines) - RTL layout styles

**JavaScript Files:**
- `wwwroot/js/language-switcher.js` (121 lines) - Language switching logic

### Database Changes

**Migration:** `20251111132441_AddArabicFields`
- Added `NameAr` to CustomerTypes, DocumentTypes, BusinessTypes
- Added `DescriptionAr` to all three tables
- All fields are NVARCHAR for proper Arabic storage

### Session Management

- Language preference stored in server-side session
- Session timeout: 24 hours
- Automatic language detection on page load

## Browser Compatibility / توافق المتصفح

Arabic RTL support works on:
- ✅ Chrome 120+ / كروم
- ✅ Firefox 115+ / فايرفوكس
- ✅ Safari 16+ / سفاري
- ✅ Edge 120+ / إيدج
- ✅ Mobile browsers / متصفحات الجوال

## Keyboard Shortcuts / اختصارات لوحة المفاتيح

### Arabic Input on Windows

1. **Add Arabic Keyboard:**
   - Settings → Time & Language → Language
   - Add Arabic (Saudi Arabia)

2. **Switch Input Language:**
   - Press `Windows + Space`
   - Or click language indicator in taskbar

3. **Type in Arabic:**
   - Select Arabic keyboard
   - Start typing in Arabic in NameAr fields

## Screenshots / لقطات الشاشة

### English Interface:
- Clean LTR layout
- Professional English typography
- Left-aligned navigation

### Arabic Interface (RTL):
- Complete RTL layout
- Beautiful Arabic typography (Tajawal font)
- Right-aligned navigation
- Arabic table headers: اسم النوع، الوصف، الحالة
- Arabic status badges: نشط / غير نشط
- Arabic buttons: إضافة، تعديل، حذف

## FAQ / أسئلة شائعة

**Q: Do I need to enter data in both languages?**
A: English name is required. Arabic is optional but recommended for better user experience.

**س: هل يجب إدخال البيانات بكلتا اللغتين؟**
ج: الاسم الإنجليزي مطلوب. العربية اختيارية لكن موصى بها لتجربة أفضل.

**Q: Can I search in Arabic?**
A: Yes! The system will search in both English and Arabic fields.

**Q: What happens if I don't enter Arabic name?**
A: The system will display the English name in Arabic mode.

**Q: Can I change the language for specific users?**
A: Currently, language is session-based. Each user can choose their preferred language.

## Support / الدعم

For issues with Arabic support:
1. Clear browser cache
2. Try different browser
3. Check that Arabic keyboard is installed
4. Verify NVARCHAR fields in database

## Accessibility / إمكانية الوصول

- ✅ Screen readers support both languages
- ✅ Keyboard navigation works in RTL
- ✅ Proper lang attributes on HTML elements
- ✅ ARIA labels (can be enhanced)

## Future Enhancements / تحسينات مستقبلية

### Planned Features:
1. **Hijri Calendar** - التقويم الهجري
2. **Arabic Number Format** - تنسيق الأرقام العربية
3. **PDF Export in Arabic** - تصدير PDF بالعربية
4. **Email Templates in Arabic** - قوالب البريد بالعربية
5. **Arabic Error Messages** - رسائل الخطأ بالعربية

---

**Developed for AEGIS Insurance & Reinsurance Brokers W.L.L**
**مطور لصالح شركة إيجيس للوساطة التأمينية**

**Supporting Both Languages / دعم كلتا اللغتين:**
- English 🇬🇧
- العربية 🇸🇦