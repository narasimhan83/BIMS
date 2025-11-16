# CRM, Policy Management & Sales Modules Architecture Plan

## Overview
This document outlines the implementation plan for adding CRM, Policy Management, and Sales modules to the BIMS application.

## New Menu Structure

### 1. CRM Menu
- **Leads**: Manage potential customers and sales opportunities
- **Proposals**: Track proposal submissions and responses

### 2. Policy Management Menu
- **Policies**: Manage insurance policies

### 3. Sales Menu
- **Sales Invoices**: Track sales transactions
- **Credit Notes**: Manage credit notes for returns/refunds
- **Customer Payments**: Track customer payment records

## Database Models

### Lead Model
```csharp
public class Lead
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string CompanyEn { get; set; }
    public string CompanyAr { get; set; }
    public string Source { get; set; } // Website, Referral, Cold Call, etc.
    public string Status { get; set; } // New, Contacted, Qualified, Proposal, Won, Lost
    public decimal PotentialValue { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastContactDate { get; set; }
    public string AssignedTo { get; set; } // User ID
}
```

### Proposal Model
```csharp
public class Proposal
{
    public int Id { get; set; }
    public int LeadId { get; set; }
    public Lead Lead { get; set; }
    public string TitleEn { get; set; }
    public string TitleAr { get; set; }
    public string DescriptionEn { get; set; }
    public string DescriptionAr { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } // Draft, Sent, Accepted, Rejected, Expired
    public DateTime CreatedDate { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ResponseDate { get; set; }
    public string ResponseNotes { get; set; }
    public DateTime ValidUntil { get; set; }
}
```

### Policy Model
```csharp
public class Policy
{
    public int Id { get; set; }
    public string PolicyNumber { get; set; }
    public int InsuranceClientId { get; set; }
    public InsuranceClient InsuranceClient { get; set; }
    public int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PremiumAmount { get; set; }
    public string Status { get; set; } // Active, Expired, Cancelled, Claimed
    public string CoverageDetailsEn { get; set; }
    public string CoverageDetailsAr { get; set; }
    public decimal SumInsured { get; set; }
    public string TermsAndConditionsEn { get; set; }
    public string TermsAndConditionsAr { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

### SalesInvoice Model
```csharp
public class SalesInvoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string Status { get; set; } // Draft, Sent, Paid, Overdue, Cancelled
    public string Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<SalesInvoiceItem> Items { get; set; }
}
```

### SalesInvoiceItem Model
```csharp
public class SalesInvoiceItem
{
    public int Id { get; set; }
    public int SalesInvoiceId { get; set; }
    public SalesInvoice SalesInvoice { get; set; }
    public string DescriptionEn { get; set; }
    public string DescriptionAr { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
```

### CreditNote Model
```csharp
public class CreditNote
{
    public int Id { get; set; }
    public string CreditNoteNumber { get; set; }
    public int SalesInvoiceId { get; set; }
    public SalesInvoice SalesInvoice { get; set; }
    public DateTime CreditNoteDate { get; set; }
    public decimal Amount { get; set; }
    public string ReasonEn { get; set; }
    public string ReasonAr { get; set; }
    public string Status { get; set; } // Draft, Approved, Applied
    public DateTime CreatedDate { get; set; }
}
```

### CustomerPayment Model
```csharp
public class CustomerPayment
{
    public int Id { get; set; }
    public string PaymentReference { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int? SalesInvoiceId { get; set; }
    public SalesInvoice SalesInvoice { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } // Cash, Bank Transfer, Cheque, Credit Card
    public string BankReference { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

## Controllers Structure

### LeadsController
- Index() - List all leads
- Create() - Create new lead
- Edit(int id) - Edit lead
- Delete(int id) - Delete lead
- Details(int id) - View lead details

### ProposalsController
- Index() - List all proposals
- Create(int leadId) - Create proposal for lead
- Edit(int id) - Edit proposal
- Delete(int id) - Delete proposal
- Details(int id) - View proposal details

### PoliciesController
- Index() - List all policies
- Create() - Create new policy
- Edit(int id) - Edit policy
- Delete(int id) - Delete policy
- Details(int id) - View policy details

### SalesInvoicesController
- Index() - List all sales invoices
- Create() - Create new sales invoice
- Edit(int id) - Edit sales invoice
- Delete(int id) - Delete sales invoice
- Details(int id) - View sales invoice details

### CreditNotesController
- Index() - List all credit notes
- Create(int salesInvoiceId) - Create credit note for invoice
- Edit(int id) - Edit credit note
- Delete(int id) - Delete credit note
- Details(int id) - View credit note details

### CustomerPaymentsController
- Index() - List all customer payments
- Create() - Create new customer payment
- Edit(int id) - Edit customer payment
- Delete(int id) - Delete customer payment
- Details(int id) - View customer payment details

## Views Structure

### CRM Views
- Views/Leads/Index.cshtml
- Views/Leads/Create.cshtml
- Views/Leads/Edit.cshtml
- Views/Leads/Delete.cshtml
- Views/Leads/Details.cshtml

- Views/Proposals/Index.cshtml
- Views/Proposals/Create.cshtml
- Views/Proposals/Edit.cshtml
- Views/Proposals/Delete.cshtml
- Views/Proposals/Details.cshtml

### Policy Management Views
- Views/Policies/Index.cshtml
- Views/Policies/Create.cshtml
- Views/Policies/Edit.cshtml
- Views/Policies/Delete.cshtml
- Views/Policies/Details.cshtml

### Sales Views
- Views/SalesInvoices/Index.cshtml
- Views/SalesInvoices/Create.cshtml
- Views/SalesInvoices/Edit.cshtml
- Views/SalesInvoices/Delete.cshtml
- Views/SalesInvoices/Details.cshtml

- Views/CreditNotes/Index.cshtml
- Views/CreditNotes/Create.cshtml
- Views/CreditNotes/Edit.cshtml
- Views/CreditNotes/Delete.cshtml
- Views/CreditNotes/Details.cshtml

- Views/CustomerPayments/Index.cshtml
- Views/CustomerPayments/Create.cshtml
- Views/CustomerPayments/Edit.cshtml
- Views/CustomerPayments/Delete.cshtml
- Views/CustomerPayments/Details.cshtml

## Navigation Menu Updates

Add to _Layout.cshtml after Insurance Management dropdown:

```html
<div class="dropdown">
    <button class="dropdown-toggle">
        <span>🎯</span> @(currentLang == "ar" ? "إدارة علاقات العملاء" : "CRM") <span class="arrow">▼</span>
    </button>
    <div class="dropdown-menu">
        <a asp-controller="Leads" asp-action="Index" class="dropdown-item">
            <span>👥</span> @(currentLang == "ar" ? "العملاء المحتملون" : "Leads")
        </a>
        <a asp-controller="Proposals" asp-action="Index" class="dropdown-item">
            <span>📋</span> @(currentLang == "ar" ? "العروض" : "Proposals")
        </a>
    </div>
</div>
<div class="dropdown">
    <button class="dropdown-toggle">
        <span>📋</span> @(currentLang == "ar" ? "إدارة الوثائق" : "Policy Management")
    </button>
    <div class="dropdown-menu">
        <a asp-controller="Policies" asp-action="Index" class="dropdown-item">
            <span>📄</span> @(currentLang == "ar" ? "الوثائق" : "Policies")
        </a>
    </div>
</div>
<div class="dropdown">
    <button class="dropdown-toggle">
        <span>💰</span> @(currentLang == "ar" ? "المبيعات" : "Sales") <span class="arrow">▼</span>
    </button>
    <div class="dropdown-menu">
        <a asp-controller="SalesInvoices" asp-action="Index" class="dropdown-item">
            <span>📄</span> @(currentLang == "ar" ? "فواتير المبيعات" : "Sales Invoices")
        </a>
        <a asp-controller="CreditNotes" asp-action="Index" class="dropdown-item">
            <span>📝</span> @(currentLang == "ar" ? "إشعارات الائتمان" : "Credit Notes")
        </a>
        <a asp-controller="CustomerPayments" asp-action="Index" class="dropdown-item">
            <span>💳</span> @(currentLang == "ar" ? "مدفوعات العملاء" : "Customer Payments")
        </a>
    </div>
</div>
```

## Implementation Order

1. Create all models with bilingual support
2. Update ApplicationDbContext
3. Create database migration
4. Create controllers with CRUD operations
5. Create views with bilingual support
6. Update navigation menu
7. Apply migration and test

## Bilingual Support

All text fields will have English (En) and Arabic (Ar) versions:
- Lead: NameEn/NameAr, CompanyEn/CompanyAr
- Proposal: TitleEn/TitleAr, DescriptionEn/DescriptionAr
- Policy: CoverageDetailsEn/CoverageDetailsAr, TermsAndConditionsEn/TermsAndConditionsAr
- SalesInvoiceItem: DescriptionEn/DescriptionAr
- CreditNote: ReasonEn/ReasonAr

## Relationships

- Lead -> Proposal (One-to-Many)
- InsuranceClient -> Policy (One-to-Many)
- ProductType -> Policy (Many-to-One)
- Customer -> SalesInvoice (One-to-Many)
- SalesInvoice -> SalesInvoiceItem (One-to-Many)
- SalesInvoice -> CreditNote (One-to-Many)
- Customer -> CustomerPayment (One-to-Many)
- SalesInvoice -> CustomerPayment (One-to-Many, optional)

## Validation Rules

- Required fields: Names, emails, dates, amounts
- Email format validation
- Date range validation (end date after start date)
- Positive amount validation
- Foreign key constraints
- Unique constraints on reference numbers

## Status Management

- Lead Status: New, Contacted, Qualified, Proposal, Won, Lost
- Proposal Status: Draft, Sent, Accepted, Rejected, Expired
- Policy Status: Active, Expired, Cancelled, Claimed
- Sales Invoice Status: Draft, Sent, Paid, Overdue, Cancelled
- Credit Note Status: Draft, Approved, Applied
- Customer Payment Status: Processed (implied by existence)

This architecture provides a comprehensive foundation for CRM, Policy Management, and Sales functionality with full bilingual support and proper data relationships.