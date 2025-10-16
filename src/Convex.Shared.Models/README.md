# Convex.Shared.Models

**Domain models and data transfer objects for Convex betting platform** - Provides reusable DTOs, enums, value objects, and constants for consistent data structures across all Convex microservices.

## 🚀 Key Features

- **📦 DTOs**: Data transfer objects for API communication
- **🔢 Enums**: Standard enumerations for consistent data types
- **💰 Value Objects**: Immutable value objects (Money, PhoneNumber, Odds)
- **📋 Constants**: Business constants and configuration values
- **✅ Validation**: Built-in data validation attributes

## 📦 Installation

```xml
<PackageReference Include="Convex.Shared.Models" Version="1.0.0" />
```

## 🚀 Quick Start

### DTOs Usage
```csharp
// User DTO
var user = new UserDto
{
    Id = Guid.NewGuid(),
    PhoneNumber = "+251912345678",
    FullName = "John Doe",
    Email = "john@example.com",
    UserType = UserType.Better,
    Balance = 1000.00m
};

// Create user request
var request = new CreateUserRequest
{
    PhoneNumber = "+251912345678",
    FullName = "John Doe",
    UserType = UserType.Better
};
```

### Value Objects Usage
```csharp
// Money value object
var money = new Money(1000.50m, "ETB");
var total = money + new Money(500.25m, "ETB"); // 1500.75 ETB

// Phone number value object
var phone = new PhoneNumber("+251912345678");
var localFormat = phone.LocalFormat; // "0912345678"
var internationalFormat = phone.InternationalFormat; // "+251912345678"

// Odds value object
var odds = new Odds(2.50m);
var potentialWin = odds.CalculatePotentialWin(100m); // 250.00
```

### Enums Usage
```csharp
// User types
var userType = UserType.Better;
var isAgent = userType == UserType.Agent;

// Bet statuses
var betStatus = BetStatus.Open;
var isWon = betStatus == BetStatus.Won;

// Transaction types
var transactionType = TransactionType.Deposit;
var isBetting = transactionType == TransactionType.BetStake;
```

### Constants Usage
```csharp
// Betting constants
var minStake = BettingConstants.MinBetStake; // 10.00
var maxWin = BettingConstants.MaxWin; // 350000.00
var maxSelections = BettingConstants.MaxSelectionsPerTicket; // 20

// Payment constants
var minDeposit = PaymentConstants.MinDepositAmount; // 10.00
var maxWithdrawal = PaymentConstants.MaxWithdrawalAmount; // 100000.00
var defaultCurrency = PaymentConstants.DefaultCurrency; // "ETB"
```

## 📚 Available DTOs

### User DTOs
- **UserDto**: User data transfer object
- **CreateUserRequest**: User creation request
- **UpdateUserRequest**: User update request

### Betting DTOs
- **TicketDto**: Betting ticket data
- **BetDto**: Individual bet data
- **PlaceBetRequest**: Bet placement request
- **BetSelectionDto**: Bet selection data

### Transaction DTOs
- **TransactionDto**: Transaction data
- **PaymentRequest**: Payment request
- **WithdrawalRequest**: Withdrawal request

### Common DTOs
- **ValidationResult**: Validation result data
- **ApiResponse<T>**: Standard API response wrapper

## 🔢 Available Enums

### User Enums
- **UserType**: Better, Agent, Client, Sales, Admin, Support
- **UserStatus**: Active, Inactive, Suspended, Banned

### Betting Enums
- **BetStatus**: Open, Won, Lost, Refunded, Cancelled
- **TicketStatus**: Pending, Done, Cancelled
- **BetType**: Single, Accumulator, System

### Transaction Enums
- **TransactionType**: Deposit, Withdrawal, BetStake, BetWin, etc.
- **PaymentStatus**: Pending, Processing, Completed, Failed, etc.
- **PaymentProvider**: MPesa, Chappa, ArifPay, Telebirr, etc.

## 💰 Value Objects

### Money
- **Amount**: Decimal value with currency
- **Currency**: ISO currency code (ETB, USD, EUR)
- **Operations**: Add, subtract, multiply, compare
- **Validation**: Non-negative amounts, supported currencies

### PhoneNumber
- **Validation**: Ethiopian phone number format
- **Normalization**: Standard format conversion
- **Formats**: Local (0912345678), International (+251912345678)

### Odds
- **Range**: 1.01 to 1000.0
- **Calculations**: Potential win, profit calculation
- **Formats**: Decimal, fractional, American odds
- **Validation**: Range and format validation

## 📋 Constants

### BettingConstants
- **Limits**: Min/max stakes, wins, selections
- **Taxes**: Withholding, TOT, VAT rates
- **Configuration**: Default currency, country code
- **Validation**: Regex patterns, limits

### PaymentConstants
- **Fees**: Transaction fees, limits
- **Timeouts**: Processing timeouts, retry attempts
- **Currencies**: Supported currencies, default currency
- **Statuses**: Final payment statuses

## ✅ Validation

All DTOs include comprehensive validation attributes:

```csharp
[Required(ErrorMessage = "Phone number is required")]
[StringLength(15, MinimumLength = 10)]
public string PhoneNumber { get; set; }

[Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
public decimal Amount { get; set; }

[EmailAddress(ErrorMessage = "Invalid email format")]
public string? Email { get; set; }
```

## 🎯 Best Practices

1. **Use DTOs**: Always use DTOs for API communication
2. **Use Value Objects**: Leverage value objects for domain concepts
3. **Use Enums**: Use enums for consistent data types
4. **Use Constants**: Use constants instead of magic numbers
5. **Validate Early**: Use validation attributes for input validation
6. **Immutable Objects**: Value objects are immutable by design
7. **Type Safety**: Use strong typing for better code quality

## 📄 License

This project is licensed under the MIT License.
