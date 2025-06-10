# Rased - Personal Financial Ecosystem Management Backend

**Rased** is a comprehensive personal finance management backend API built on .NET Core 9.0.200 that empowers users to organize their financial life through multiple customizable wallets, each representing different aspects of their financial ecosystem.

## Technology Stack

**Backend Framework**: .NET Core 9.0.200  
**Architecture**: 3-Tier Architecture (Repository - Service - Application Layer)  
**ORM**: Entity Framework Core for data access  
**API Design**: RESTful API with comprehensive endpoint coverage for financial data management  
**AI Integration**: Smart document processing and attachment analysis  
**Security**: Enterprise-grade authentication and data protection

## What is Rased Backend?

Rased backend API transforms personal finance management from a single-account approach to a multi-wallet ecosystem where client applications can create, manage, and synchronize different financial contexts. The API provides comprehensive endpoints for wallet management, financial tracking, user collaboration, and intelligent expense processing.

## AI-Powered Financial Intelligence

### Automated Expense Processing
- **Document Scanning**: AI-powered receipt and invoice processing that automatically extracts amounts, dates, merchant names, and expense categories
- **Smart Categorization**: Machine learning algorithms that learn from user behavior to automatically categorize expenses with high accuracy
- **Attachment Analysis**: Support for image and PDF processing to extract financial data from uploaded documents


## API Architecture: 


Unlike traditional finance apps that treat all money as one pool, Rased recognizes that people naturally think about money in different contexts. Each wallet in Rased operates as an independent financial environment with its own:

- **Visual Identity**: Custom colors and icons for instant recognition
- **Currency System**: Support for different currencies per wallet
- **Financial Tracking**: Complete income, expense, and budget management
- **Goal Setting**: Personalized financial objectives and progress tracking
- **Savings Management**: Track various saving instruments (gold, foreign currency, investments)
- **Transfer System**: Move money between your wallets or send to friends
- **Activity Status**: Enable or disable wallets as your financial needs change

## Key Features

### Individual Wallet Management
- **Budget Planning**: Set and monitor budgets with category-based tracking
- **Income Tracking**: Record and categorize all income sources
- **Expense Management**: Detailed expense logging with categorization
- **Financial Goals**: Set objectives like "increase income by 20%" or "reduce dining expenses"
- **Savings Portfolio**: Track different saving types including precious metals, foreign currencies, and other assets

### Social Financial Features
- **Friend Network**: Connect with other Rased users for financial collaboration
- **Shared Wallets**: Create collaborative wallets for shared expenses, group trips, or household budgets
- **Peer Transfers**: Send money directly to friends with full transaction tracking
- **Transaction History**: Complete audit trail of all transfers between friends

### Wallet Customization
- **Visual Personalization**: Choose colors and icons that represent each wallet's purpose
- **Multi-Currency Support**: Each wallet can operate in its preferred currency
- **Flexible Organization**: Create wallets for different purposes (Personal, Business, Travel, Shared Expenses)
- **Status Management**: Activate or deactivate wallets based on current needs

## Use Cases

**Personal Organization**: Separate personal spending from business expenses with dedicated wallets for each context.

**Family Financial Management**: Create shared wallets for household expenses while maintaining individual wallets for personal spending.

**Group Expense Tracking**: Manage shared expenses for roommates, group trips, or collaborative projects with transparent tracking and easy settlement.

**Multi-Currency Management**: Handle different currencies for international travel, foreign investments, or cross-border transactions.

**Goal-Oriented Saving**: Set up dedicated wallets for specific financial goals like emergency funds, vacation savings, or major purchases.

**Investment Tracking**: Monitor various saving and investment vehicles including traditional savings, precious metals, and foreign currency holdings.

## Security Architecture

### Authentication & Authorization
- **JWT with Refresh Tokens**: Secure token-based authentication with automatic refresh mechanism for seamless user experience
- **Email Verification**: Mandatory email verification for account activation and critical operations
- **Account Protection**: Automatic account lockout after 4 failed login attempts for 15 minutes to prevent brute force attacks

### Password Security
- **High-Security Requirements**: Enforced complex password policies with minimum length, character variety, and strength validation
- **Salt-Based Hashing**: Password hashing with unique salts to prevent rainbow table attacks
- **Secure Password Reset**: Multi-step password reset process with time-limited tokens

### Application Security
- **SQL Injection Prevention**: Parameterized queries and ORM-based data access to eliminate SQL injection vulnerabilities
- **XSS Protection**: Input validation and output encoding to prevent cross-site scripting attacks
- **CSRF Protection**: Anti-forgery tokens for state-changing operations
- **Rate Limiting**: API endpoint rate limiting to prevent abuse and ensure service availability
- **Data Encryption**: Sensitive financial data encrypted at rest and in transit
- **Input Validation**: Comprehensive server-side validation for all user inputs and file uploads

### Financial Data Protection
- **Data Encryption**: Sensitive financial data encrypted at rest and in transit
- **Data Anonymization**: User data anonymization capabilities for privacy compliance

## API Endpoints Overview

### Wallet Management API
- **CRUD Operations**: Complete wallet lifecycle management with validation
- **Multi-Currency Support**: Currency conversion and exchange rate integration
- **Wallet Sharing**: Friend network integration and shared wallet management
- **Status Management**: Wallet activation/deactivation with data preservation

### Financial Tracking API
- **Budget Management**: Budget creation, monitoring, and variance reporting
- **Income Tracking**: Multiple income source management with categorization
- **Expense Processing**: Detailed expense logging with AI-assisted categorization
- **Transfer System**: Secure peer-to-peer transfers with transaction verification

### AI Integration API
- **Document Upload**: Secure file upload with virus scanning and validation
- **OCR Processing**: Receipt and invoice data extraction endpoints

## Quick Start Guide

### Prerequisites
- .NET Core 9.0.200 SDK
- SQL Server (LocalDB for development)

### Installation & Setup

```bash
# 1. Clone the repository
git clone [repository-url]
cd rased-backend

# 2. Restore dependencies
dotnet restore

# 3. Update connection string in appsettings.json
# Modify the DefaultConnection string to point to your SQL Server instance

# 4. Create and apply database migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# 5. Run the project
dotnet run
```

### Configuration
Update your `appsettings.json` with appropriate connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RasedDB;Trusted_Connection=true;"
  }
}
```
- **Authentication**: Login, logout, token refresh, and session management
- **Profile Management**: User profile CRUD operations with validation
- **Friend Network**: User discovery, friend requests, and relationship management
- **Security Operations**: Password change, email verification, and account recovery
