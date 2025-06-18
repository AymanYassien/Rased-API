# Rased Backend API - راصد

![Rased Banner](https://github.com/user-attachments/assets/d3c0e412-6522-4bc0-8af8-0bc3b9551aba)

> **Intelligent Financial Analytics Backend** - Powering smart money management with AI-driven insights and secure collaborative features.

## 🚀 Overview

Rased Backend is the core API service that powers the Rased financial analytics application. Built with modern technologies, it provides secure, scalable, and intelligent financial management capabilities including AI-powered invoice processing, collaborative wallet management, and personalized financial recommendations.

## ✨ Key Features

### 🔐 **Authentication & Security**
- JWT-based authentication with refresh tokens
- Two-Factor Authentication (2FA) via email OTP
- Password reset and account recovery
- Role-based access control
- Data encryption and secure API endpoints

### 💰 **Wallet Management**
- Personal wallet creation and management
- Shared wallet functionality with multi-user access
- Real-time balance tracking
- Transaction categorization and filtering
- Comprehensive expense and income tracking

### 🤖 **AI-Powered Features**
- **Computer Vision OCR**: Automatic invoice data extraction from images
- **Recommendation Engine**: Personalized financial advice and strategies

### 🎯 **Financial Tools**
- Goal setting and progress tracking
- Savings management with automated calculations
- Loan management with strategic repayment plans
- Budget creation and monitoring
- Financial reporting and analytics

### 👥 **Social Features**
- User friendship system
- Experience sharing between users
- Collaborative wallet invitations
- User profile management

### 💸 **Money Transfer**
- Secure peer-to-peer transfers
- Inter-wallet transfers
- Transaction history tracking
- Real-time balance updates

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0 Web API
- **Architecture**: 3-Tier Architecture (API, Business, Infrastructure)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer Token + ASP.NET Core Identity
- **AI/ML**: Gemini LLM, Computer Vision
- **Email Service**: SMTP with MailKit
- **Validation**: FluentValidation
- **Testing**: xUnit, Moq, FluentAssertions
- **Documentation**: Swagger/OpenAPI with Swashbuckle
- **Dependency Injection**: ASP.NET Core DI Container

## 📁 Project Structure (3-Tier Architecture)

```
rased-backend/
├── Rased.Api/                      # Presentation Layer
│   ├── Controllers/               # API Controllers
│   ├── Middlewares/              # Custom middlewares
│   ├── wwwroot/                 # Static files
│   ├── appsettings.json         # Configuration
│   ├── Program.cs               # Application entry point and Service configuration
│
├── Rased.Business/                 # Business Logic Layer
│   ├── Services/                # Business services
│   │   ├── WalletService.cs
│   │   ├── TransactionService.cs
│   │   ├── AIService.cs
│   │   ├── AuthService.cs
│   │   └── RecommendationService.cs
│   ├── DTOs/                    # Data Transfer Objects
│   └── Mappers/              # AutoMapper profiles
│
├── Rased.Infrastructure/           # Data Access Layer
│   ├── Data/                    # DbContext and configurations
│   ├── Repositories/            # Repository implementations
│   ├── Entities/                # Database entities
│   ├── Migrations/              # EF Core migrations
│   ├── Models/                  # System Entities
│   ├── Helpers/                 # Helpers classes
│   └── UnitOfWork/              # Unit Of Work Design Pattern
│
│
└── Rased.sln                      # Solution file
```

## 🚦 Getting Started

### Prerequisites
- .NET 9.0 SDK or higher
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code with C# extension
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/rased-backend.git
   cd rased-backend
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Database Setup**
   ```bash
   # Update connection string in appsettings.json
   # Run migrations
   dotnet ef database update --project Rased.Infrastructure --startup-project Rased.Api
   ```

4. **Environment Setup**
   Update `appsettings.json` and `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RasedDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     },
     "JwtSettings": {
       "SecretKey": "your-super-secret-jwt-key-here",
       "Issuer": "RasedApi",
       "Audience": "RasedClient",
       "ExpirationInMinutes": 60
     },
     "EmailSettings": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": 587,
       "Username": "your-email@gmail.com",
       "Password": "your-app-password"
     },
     "AISettings": {
       "OcrEndpoint": "your-azure-cognitive-services-endpoint",
       "OcrApiKey": "your-ocr-api-key"
     }
   }
   ```

5. **Start the application**
   ```bash
   # Development mode
   dotnet run --project Rased.Api
   
   # Or using Visual Studio - Set Rased.Api as startup project
   ```

## 📚 API Documentation

### Base URL
```
https://localhost:7001/api/v1
```

### Authentication Endpoints
```http
POST /api/v1/auth/register          # User registration
POST /api/v1/auth/login             # User login
POST /api/v1/auth/refresh-token     # Refresh JWT token
POST /api/v1/auth/forgot-password   # Request password reset
POST /api/v1/auth/reset-password    # Reset password
POST /api/v1/auth/verify-2fa        # Verify 2FA code
POST /api/v1/auth/logout            # User logout
```

### Wallet Endpoints
```http
GET    /api/v1/wallets              # Get user wallets
POST   /api/v1/wallets              # Create new wallet
GET    /api/v1/wallets/{id}         # Get wallet details
PUT    /api/v1/wallets/{id}         # Update wallet
DELETE /api/v1/wallets/{id}         # Delete wallet
POST   /api/v1/wallets/{id}/invite  # Invite user to shared wallet
```

### AI Endpoints
```http
POST   /api/v1/ai/process-invoice   # OCR invoice processing
GET    /api/v1/ai/recommendations   # Get financial recommendations
POST   /api/v1/ai/analyze-spending  # Analyze spending patterns
```

For complete API documentation, visit: `http://rased-api.runasp.net/swagger/index.html`

### Production Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-production-sql-connection-string"
  },
  "JwtSettings": {
    "SecretKey": "your-production-jwt-secret-key",
    "ExpirationInMinutes": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

Graduation Project 2025, FCI - KFSU

## 👥 Team

- **Mohamed Adel Elsayed** - FullStack Engineer
- **Ayman Mohamed** - Backend Engineer
- **Fawzy Shaker** - Backend Engineer
- **Mohamed Elmahallawy** - CyberSecurity Engineer
- **Basem Mohamed** - Frontend Engineer
- **Basant Selima** - Product Designer

## 📞 Support

For support and questions:
- 📧 Email: rased.fci@gmail.com
- 📖 Documentation: [Swagger API Docs](http://rased-api.runasp.net/swagger/index.html)

---

**⭐ Star this repo if you find it helpful!**
