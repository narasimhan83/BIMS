# AEGIS Insurance Broker Management System (IBMS)

## 🎯 Project Overview

A modern ASP.NET Core 8.0 MVC application for AEGIS Insurance & Reinsurance Brokers W.L.L featuring:
- ✨ Beautiful glassmorphism login UI with AEGIS branding
- 🔐 ASP.NET Core Identity authentication
- 🎨 Modern teal color theme matching the AEGIS logo
- 📱 Fully responsive design
- 🔒 Secure password-based authentication

## 🚀 Quick Start Guide

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server (your remote server at 87.252.104.168)
- Visual Studio 2022 or VS Code

### Step 1: Restore NuGet Packages

```bash
dotnet restore
```

### Step 2: Create Database Migration

```bash
dotnet ef migrations add InitialCreate
```

This will create the migration files in the `Migrations` folder with all necessary Identity tables.

### Step 3: Apply Migration to Database

```bash
dotnet ef database update
```

This will:
- Create the IBMS database on your SQL Server (87.252.104.168)
- Create all ASP.NET Identity tables (AspNetUsers, AspNetRoles, etc.)
- Add custom fields to ApplicationUser (FullName, CreatedDate, LastLoginDate, IsActive)

### Step 4: Run the Application

```bash
dotnet run
```

Or press F5 in Visual Studio.

The application will start at: `https://localhost:5001` or `http://localhost:5000`

## 📋 Initial Setup & Testing

### 1. Register First User
1. Navigate to the application URL
2. You'll be redirected to the login page
3. Click "Register here" link
4. Fill in the registration form:
   - Full Name: Your name
   - Username: Choose a username
   - Password: Minimum 6 characters
   - Confirm Password: Same as password
5. Click "Create Account"

### 2. Login
1. Enter your username and password
2. Check "Remember me" if desired
3. Click "Sign In"
4. You'll be redirected to the Dashboard

### 3. Explore Dashboard
The dashboard shows:
- Welcome message with your username
- 4 metric cards (empty for now):
  - Clients
  - Policies
  - Premium
  - Claims
- Welcome card with action buttons

## 🎨 Design Features

### Login Page
- **Glassmorphism Effect**: Frosted glass card with blur backdrop
- **Animated Gradient Background**: Smooth teal gradient animation
- **Floating Shapes**: Subtle background animations
- **Smooth Transitions**: All interactive elements have smooth hover effects
- **Form Validation**: Client-side validation with clear error messages

### Dashboard
- **Modern Card Layout**: Grid of metric cards with icons
- **Responsive Design**: Works on mobile, tablet, and desktop
- **Professional Navigation**: Header with AEGIS logo and logout button
- **Color Scheme**: Consistent teal theme throughout

## 🎨 Color Palette

Based on the AEGIS logo:

| Color Name | Hex Code | Usage |
|------------|----------|-------|
| Primary Teal | `#00A6A6` | Buttons, accents |
| Teal Light | `#4DB8B8` | Hover states |
| Teal Dark | `#008B8B` | Button hover |
| Navy Dark | `#0D4D4D` | Headers, text |
| Navy Medium | `#1A5C5C` | Secondary elements |
| Cyan Light | `#7DD5D5` | Highlights, borders |

## 📁 Project Structure

```
BIMS/
├── Controllers/
│   ├── AccountController.cs      # Authentication logic
│   └── HomeController.cs          # Dashboard
├── Models/
│   ├── ApplicationUser.cs         # Extended Identity user
│   ├── LoginViewModel.cs          # Login form
│   ├── RegisterViewModel.cs       # Registration form
│   └── ErrorViewModel.cs
├── Data/
│   └── ApplicationDbContext.cs    # EF Core context
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml           # Login page
│   │   └── Register.cshtml        # Registration page
│   ├── Home/
│   │   └── Dashboard.cshtml       # Main dashboard
│   └── Shared/
│       ├── _Layout.cshtml         # Dashboard layout
│       └── _LoginLayout.cshtml    # Login layout
├── wwwroot/
│   ├── css/
│   │   ├── login.css              # Login page styles
│   │   └── site.css               # Dashboard styles
│   └── images/
│       └── aegis-logo.png         # Company logo
├── Migrations/                     # EF Core migrations
├── Program.cs                      # Application configuration
├── appsettings.json               # Configuration
└── BIMS.csproj                    # Project file
```

## 🔐 Security Features

- ✅ Password hashing (ASP.NET Identity default - PBKDF2)
- ✅ CSRF protection (Anti-Forgery tokens)
- ✅ XSS prevention (Razor automatic encoding)
- ✅ SQL injection prevention (Entity Framework parameterization)
- ✅ Secure session management
- ✅ HTTPS enforcement (production)

## 📱 Responsive Breakpoints

- **Mobile**: < 576px - Single column layout
- **Tablet**: 576px - 768px - Adjusted spacing
- **Desktop**: 768px - 1200px - Full layout
- **Large**: > 1200px - Maximum width container

## 🔧 Configuration

### Database Connection String
Located in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=87.252.104.168;Database=IBMS;User Id=sa;Password=*26malar19baby;Encrypt=false;MultipleActiveResultSets=true;"
  }
}
```

**⚠️ Security Note**: For production, move sensitive connection strings to User Secrets or environment variables.

### Identity Settings
Configured in `Program.cs`:
- Minimum password length: 6 characters
- No complexity requirements (configurable)
- Account lockout: Disabled (can be enabled)
- Session timeout: 24 hours

## 🐛 Troubleshooting

### Migration Issues

**Problem**: Migration command fails
```bash
# Solution: Install/Update EF Core tools
dotnet tool install --global dotnet-ef
# Or update
dotnet tool update --global dotnet-ef
```

**Problem**: Cannot connect to database
- Verify SQL Server is running on 87.252.104.168
- Check firewall rules allow connection
- Verify credentials are correct

### Build Issues

**Problem**: Build fails
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Runtime Issues

**Problem**: 404 Not Found on login
- Ensure migrations are applied
- Check Program.cs routing configuration

**Problem**: CSS not loading
- Check wwwroot folder structure
- Verify file paths in HTML

## 🚀 Next Steps

After initial setup, you can extend the application with:

1. **User Management**
   - Admin role
   - User profile editing
   - Password reset

2. **Business Modules**
   - Client management
   - Policy management
   - Claims processing
   - Premium calculations

3. **Enhanced Features**
   - Email notifications
   - Document upload
   - Reporting & analytics
   - Export to Excel/PDF

4. **Security Enhancements**
   - Two-factor authentication
   - Email verification
   - Password complexity rules
   - Session timeout warnings

## 📞 Support

For issues or questions about AEGIS IBMS, please contact your development team.

## 📄 License

Copyright © 2024 AEGIS Insurance & Reinsurance Brokers W.L.L

---

**Built with ❤️ using ASP.NET Core 8.0**