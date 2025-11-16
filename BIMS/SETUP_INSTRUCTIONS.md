# 🚀 AEGIS IBMS - Quick Setup Instructions

## ⚡ Run These 3 Commands to Get Started

Open a terminal in the project directory and run:

### 1. Create Database Migration
```bash
dotnet ef migrations add InitialCreate
```
This creates the migration files for Identity tables.

### 2. Apply Migration to Database
```bash
dotnet ef database update
```
This creates the IBMS database and all tables on your SQL Server (87.252.104.168).

### 3. Run the Application
```bash
dotnet run
```
Then open your browser to `https://localhost:5001` or `http://localhost:5000`

## 🎯 First Time Use

1. **Register**: Click "Register here" on the login page
2. **Create Account**: Fill in your details (username, password, full name)
3. **Login**: Use your credentials to sign in
4. **Dashboard**: You'll land on the beautiful AEGIS dashboard!

## ✨ What You Get

- 🎨 **Stunning Login Page** with glassmorphism effects
- 🔐 **Secure Authentication** with ASP.NET Core Identity
- 📊 **Modern Dashboard** with metric cards
- 🎨 **AEGIS Brand Colors** throughout the UI
- 📱 **Fully Responsive** - works on all devices
- ⚡ **Smooth Animations** and transitions

## 📝 Login Page Features

- **Animated gradient background** in AEGIS teal colors
- **Floating shapes** with smooth animations
- **Glassmorphism card** with backdrop blur
- **Form validation** with clear error messages
- **Remember me** functionality
- **Smooth hover effects** on all interactive elements

## 🎨 AEGIS Color Theme

All pages use the AEGIS brand colors:
- **Primary Teal**: #00A6A6
- **Light Teal**: #4DB8B8
- **Dark Navy**: #0D4D4D

## 🔧 Troubleshooting

### If migration fails:
```bash
# Install EF Core tools
dotnet tool install --global dotnet-ef
```

### If database connection fails:
- Verify SQL Server is accessible at 87.252.104.168
- Check firewall settings
- Verify database credentials

## 📞 Need Help?

See the full [README.md](README.md) for detailed documentation.

---

**Ready to go! Run the 3 commands above and start using AEGIS IBMS! 🎉**