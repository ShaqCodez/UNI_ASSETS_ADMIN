# Campus Asset Tagging & Inventory System

A comprehensive two-part system designed to automate and simplify asset verification at universities. The system consists of an Android mobile application for field staff and a web-based administrative dashboard built with ASP.NET.

## 📱 System Overview

### Mobile Application (Android)
- **QR Code Scanning** - Uses ML Kit for fast, accurate barcode detection
- **Photo Capture** - Document asset condition with integrated camera
- **Offline Support** - SQLite local storage for areas with poor connectivity  
- **Form Input** - Capture asset condition, location, and notes
- **Real-time Sync** - Submit data to backend via REST API

### Web Application (ASP.NET)
- **Admin Dashboard** - Comprehensive asset management interface
- **Asset Verification** - Agent-based verification workflow
- **Advanced Search** - Filter by tag, room, condition, or custom criteria
- **Report Generation** - Customizable formatted reports
- **SQL Server Integration** - Enterprise-grade data storage with Entity Framework

## 🏗️ Architecture

```
📱 Android App (Java)     →     🌐 ASP.NET Web App (C#)     →     🗄️ SQL Server Database
├── ML Kit Scanner              ├── REST API Endpoints              ├── Asset Records
├── SQLite (Offline)            ├── Admin Dashboard                 ├── User Management  
├── Camera Integration          ├── Verification System             └── Audit Logs
└── Retrofit HTTP Client        └── Report Generation               
```

## 🚀 Features

### Mobile App Features
- [x] **QR Code Scanner** - ML Kit barcode scanning with flashlight support
- [x] **Asset Photography** - High-quality image capture with preview
- [x] **Offline-First Design** - Works without internet connectivity
- [x] **Form Validation** - Comprehensive input validation and error handling
- [x] **Material Design UI** - Modern, intuitive blue and white theme
- [x] **Real-time Feedback** - Success/error dialogs with smooth animations

### Web Application Features
- [x] **Asset Management** - Create, read, update, delete asset records
- [x] **Verification Workflow** - Agent-based approval system
- [x] **Advanced Search & Filtering** - Multi-criteria search capabilities
- [x] **Report Generation** - PDF/Excel export functionality
- [x] **User Authentication** - Secure login with role-based access
- [x] **Audit Trail** - Complete history of asset changes

## 🛠️ Technology Stack

### Android Application
- **Language:** Java
- **IDE:** Android Studio
- **Minimum SDK:** 26 (Android 8.0)
- **Target SDK:** 36 (Latest)

#### Key Libraries
```gradle
// ML Kit & Camera
implementation("com.google.mlkit:barcode-scanning:17.2.0")
implementation("androidx.camera:camera-core:1.3.1")

// Network & Storage
implementation("com.squareup.retrofit2:retrofit:2.9.0")
implementation("androidx.room:room-runtime:2.6.1")

// UI Components
implementation("com.google.android.material:material:1.11.0")
```

### Web Application
- **Framework:** ASP.NET Core
- **Language:** C#
- **Database:** SQL Server with Entity Framework Core
- **Frontend:** Razor Pages with Bootstrap
- **API:** RESTful endpoints for mobile integration

## 📋 Prerequisites

### Development Environment
- **Android Studio** Arctic Fox or later
- **Visual Studio 2022** or VS Code with C# extension
- **SQL Server** LocalDB or SQL Server Express
- **.NET 6.0** or later SDK

### Hardware Requirements
- **Android Device/Emulator** with camera support
- **Development PC** with minimum 8GB RAM
- **Camera** for QR code testing

## 🚀 Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/campus-asset-tracker.git
cd campus-asset-tracker
```

### 2. Setup Android App
```bash
cd mobile-app
# Open in Android Studio
# Sync Gradle dependencies
# Connect Android device or start emulator
# Run the app
```

### 3. Setup Web Application
```bash
cd web-app
dotnet restore
dotnet ef database update
dotnet run
# Navigate to https://localhost:5001
```

### 4. Local Development Configuration
```java
// In Android app - Update API base URL
private static final String BASE_URL = "http://10.0.2.2:5000/api/";
```

## 📖 Usage Guide

### For Field Staff (Mobile App)

1. **Login** with staff credentials
2. **Scan QR Code** on asset tag
3. **Take Photo** of current asset condition
4. **Fill Form** with:
   - Asset condition (Good/Damaged/Missing)
   - Current location
   - Additional notes
5. **Submit** verification (saves locally if offline)

### For Administrators (Web App)

1. **Dashboard** - Overview of all assets and recent activity
2. **Asset Management** - Add, edit, or remove assets from inventory
3. **Verification Queue** - Review and approve staff submissions
4. **Search & Filter** - Find assets by various criteria
5. **Generate Reports** - Export data in multiple formats

## 🔧 Configuration

### Android App Configuration
```xml
<!-- AndroidManifest.xml -->
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.INTERNET" />

<!-- ML Kit automatic model download -->
<meta-data android:name="com.google.mlkit.vision.DEPENDENCIES" 
           android:value="barcode" />
```

### Web App Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CampusAssets;Trusted_Connection=true;"
  },
  "ApiSettings": {
    "AllowedOrigins": ["http://10.0.2.2:5000", "https://localhost:5001"]
  }
}
```

## 📊 Database Schema

### Key Entities
- **Assets** - Core asset information and metadata
- **Verifications** - Staff submissions and verification records
- **Users** - Staff and administrator accounts
- **Locations** - Campus rooms and areas
- **AuditLogs** - Complete change history

## 🧪 Testing

### Android App Testing
```bash
# Unit tests
./gradlew test

# Instrumentation tests
./gradlew connectedAndroidTest

# Manual testing checklist:
# ✓ QR code scanning in various lighting
# ✓ Camera functionality and photo quality
# ✓ Offline data storage and sync
# ✓ Form validation and error handling
```

### Web App Testing
```bash
# Unit tests
dotnet test

# Integration tests
dotnet test --logger trx --results-directory TestResults/

# Manual testing checklist:
# ✓ API endpoints respond correctly
# ✓ Database operations work as expected
# ✓ Report generation functions properly
# ✓ Authentication and authorization
```

## 🚢 Deployment

### Android App Deployment
1. **Generate signed APK** in Android Studio
2. **Test on multiple devices** and screen sizes
3. **Upload to internal testing** platform
4. **Distribute to staff** via APK or app store

### Web App Deployment
1. **Publish to production** server
2. **Configure SQL Server** connection
3. **Update API endpoints** in mobile app
4. **Setup SSL certificates** for security

## 🤝 Contributing

1. **Fork** the repository
2. **Create feature branch** (`git checkout -b feature/amazing-feature`)
3. **Commit changes** (`git commit -m 'Add amazing feature'`)
4. **Push to branch** (`git push origin feature/amazing-feature`)
5. **Open Pull Request**

### Code Style Guidelines
- **Android:** Follow Google Java Style Guide
- **Web:** Follow Microsoft C# Coding Conventions
- **Git:** Use conventional commit messages

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## 👥 Team

- **[Oratilwe S]** - Android Development & Project Lead
- **[Makubo LM]** - Web Development & Database Design
- **[Lecturer Name]** - Project Supervisor

## 🆘 Support

### Common Issues
- **Camera not working?** Check permissions in device settings
- **QR codes not scanning?** Ensure adequate lighting and steady hands
- **App won't connect to server?** Verify network configuration and server status
- **Data not syncing?** Check internet connection and try manual sync

### Getting Help
- **Create an issue** on GitHub
- **Check documentation** in `/docs` folder
- **Contact team** via university email

## 🔄 Project Status

- [x] **Week 3** - Project proposal approved
- [x] **Week 7** - Mobile application completed
- [ ] **Week 12** - Web application completion
- [ ] **Final** - Integration testing and deployment

## 📈 Future Enhancements

- [ ] **Bulk QR Code Generation** - Generate printable asset tags
- [ ] **Advanced Analytics** - Asset utilization and trend analysis  
- [ ] **Mobile Web App** - PWA for cross-platform compatibility
- [ ] **Push Notifications** - Real-time alerts for administrators
- [ ] **Asset History Timeline** - Visual asset lifecycle tracking
- [ ] **Integration APIs** - Connect with existing campus systems

---

## 🙋‍♂️ FAQ

**Q: Does the app work without internet?**
A: Yes! The app stores data locally and syncs when connection is restored.

**Q: What QR code formats are supported?**
A: Standard QR codes, DataMatrix, and most common barcode formats via ML Kit.

**Q: Can I customize the asset verification form?**
A: Yes, both the mobile form and web dashboard are configurable.

**Q: Is the system secure?**
A: Yes, includes authentication, encrypted connections, and audit logging.

---
## 📝Author
- **Name:** Oratilwe Seleke  
- **Student Number:** 2023478387

- **Name:** Makubo Lebohang  
- **Student Number:** 2023350180
