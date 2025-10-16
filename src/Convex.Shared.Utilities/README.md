# Convex.Shared.Utilities

**Utility functions and helper methods for Convex betting platform** - Provides reusable utility functions for common operations across all Convex microservices.

## 🚀 Key Features

- **📱 Phone Utilities**: Phone number validation, formatting, and normalization
- **💰 Financial Calculators**: Tax calculations, fee calculations, win calculations
- **✅ Validation Helpers**: Input validation, limit validation, business rule validation
- **📝 Extended String Helpers**: Additional string utilities (random generation, masking, formatting)
- **🗄️ Extended Cache Helpers**: Additional cache key generation and management
- **🔧 Pure Functions**: No business logic, only utility operations
- **📦 Dependencies**: Uses Convex.Shared.Common for basic string extensions and cache constants

## 📦 Installation

```xml
<PackageReference Include="Convex.Shared.Utilities" Version="1.0.0" />
```

## 🚀 Quick Start

### Phone Number Utilities
```csharp
// Validate phone number
var isValid = PhoneNumberHelper.IsValidPhoneNumber("+251912345678");

// Format to international
var international = PhoneNumberHelper.FormatToInternational("0912345678");

// Format to local
var local = PhoneNumberHelper.FormatToLocal("+251912345678");

// Normalize phone number
var normalized = PhoneNumberHelper.NormalizePhoneNumber("0912345678");
```

### Financial Calculations
```csharp
// Calculate TOT (10% of stake)
var tot = FinancialCalculator.CalculateTot(100m); // 10.00

// Calculate transaction fee
var fee = FinancialCalculator.CalculateTransactionFee(1000m, 0.02m); // 20.00

// Calculate potential win
var potentialWin = FinancialCalculator.CalculatePotentialWin(100m, 2.5m); // 250.00

// Calculate total odds for accumulator
var totalOdds = FinancialCalculator.CalculateTotalOdds(new[] { 2.0m, 1.5m, 3.0m }); // 9.00
```

### Validation Helpers
```csharp
// Validate stake limits
var isValidStake = ValidationHelper.ValidateStakeLimits(50m, 10m, 1000m);

// Validate daily withdrawal limit
var canWithdraw = ValidationHelper.ValidateDailyWithdrawalLimit(500m, 1000m);

// Validate odds range
var isValidOdds = ValidationHelper.ValidateOddsRange(2.5m, 1.01m, 1000m);

// Validate string length
var isValidLength = ValidationHelper.ValidateStringLength("test", 1, 10);
```

### Extended Cache Helpers
```csharp
// Create ticket cache key
var ticketKey = ExtendedCacheHelper.CreateTicketKey(ticketId);

// Create transaction cache key
var transactionKey = ExtendedCacheHelper.CreateTransactionKey(transactionId);

// Create custom cache key
var customKey = ExtendedCacheHelper.CreateCustomKey("custom", "identifier");

// Create key with environment prefix
var envKey = ExtendedCacheHelper.CreateKeyWithEnvironmentPrefix("mykey");

// Use Common constants for user/session keys
var userKey = ExtendedCacheHelper.CreateUserKey(userId);
var sessionKey = ExtendedCacheHelper.CreateSessionKey(sessionId);
```

### Extended String Utilities
```csharp
// String generation
var randomString = ExtendedStringHelper.GenerateRandomString(10);
var randomNumeric = ExtendedStringHelper.GenerateRandomNumeric(6);

// String masking
var maskedPhone = ExtendedStringHelper.MaskPhoneNumber("+251912345678"); // "+251***45678"
var maskedEmail = ExtendedStringHelper.MaskEmail("user@example.com"); // "us***@example.com"

// String formatting
var camelCase = ExtendedStringHelper.ToCamelCase("Hello World"); // "helloWorld"
var pascalCase = ExtendedStringHelper.ToPascalCase("hello world"); // "HelloWorld"

// String cleaning
var alphanumeric = ExtendedStringHelper.RemoveNonAlphanumeric("abc123!@#"); // "abc123"
var numeric = ExtendedStringHelper.RemoveNonNumeric("abc123!@#"); // "123"
```

### Basic String Utilities (from Convex.Shared.Common)
```csharp
// Use existing string extensions from Convex.Shared.Common
var isNotEmpty = "test".IsNullOrEmpty();
var isNotWhiteSpace = "test".IsNullOrWhiteSpace();
var titleCase = "hello world".ToTitleCase(); // "Hello World"
var slug = "My Blog Post".ToSlug(); // "my-blog-post"
var isValidEmail = "user@example.com".IsValidEmail();
```

## 📚 Available Utilities

### Phone Number Helpers
- **IsValidPhoneNumber**: Validates phone number format
- **FormatToInternational**: Formats to international format (+251...)
- **FormatToLocal**: Formats to local format (0...)
- **FormatWithoutCountryCode**: Removes country code
- **NormalizePhoneNumber**: Normalizes to standard format
- **GetCountryCode**: Extracts country code
- **GetLocalNumber**: Extracts local number

### Financial Calculators
- **CalculateTot**: Calculates TOT (10% of stake)
- **CalculateWinTax**: Calculates winning tax
- **CalculateMaxWin**: Calculates maximum win amount
- **CalculateWonValue**: Calculates net won value
- **CalculateTransactionFee**: Calculates transaction fees
- **CalculatePotentialWin**: Calculates potential win amount
- **CalculateTotalOdds**: Calculates total odds for accumulator
- **RoundAmount**: Rounds amount to 2 decimal places

### Validation Helpers
- **ValidateStakeLimits**: Validates stake within limits
- **ValidateDailyWithdrawalLimit**: Validates daily withdrawal limit
- **ValidateSingleDepositLimit**: Validates single deposit limit
- **ValidatePendingStakeLimit**: Validates pending stake limit
- **ValidateOddsRange**: Validates odds within range
- **ValidatePositiveAmount**: Validates positive amount
- **ValidateStringLength**: Validates string length
- **ValidateFutureDate**: Validates future date
- **ValidateDateWithinWindow**: Validates date within time window

### Extended Cache Helpers
- **CreateTicketKey**: Creates ticket cache key
- **CreateTransactionKey**: Creates transaction cache key
- **CreateConfigurationKey**: Creates configuration cache key
- **CreateCustomKey**: Creates custom cache key
- **CreateKeyWithEnvironmentPrefix**: Creates key with environment prefix
- **GetCacheKeyPrefix**: Gets cache key prefix from environment
- **CreateUserKey**: Creates user cache key (uses Common constants)
- **CreateSessionKey**: Creates session cache key (uses Common constants)

### Extended String Helpers
- **GenerateRandomString**: Generates random string
- **GenerateRandomAlphanumeric**: Generates random alphanumeric string
- **GenerateRandomNumeric**: Generates random numeric string
- **MaskString**: Masks sensitive strings
- **MaskPhoneNumber**: Masks phone numbers
- **MaskEmail**: Masks email addresses
- **RemoveNonAlphanumeric**: Removes non-alphanumeric characters
- **RemoveNonNumeric**: Removes non-numeric characters
- **CapitalizeWords**: Capitalizes first letter of each word
- **ToCamelCase**: Converts to camelCase
- **ToPascalCase**: Converts to PascalCase

### Basic String Helpers (from Convex.Shared.Common)
- **IsNullOrEmpty**: Checks if string is null or empty
- **IsNullOrWhiteSpace**: Checks if string is null or whitespace
- **ToTitleCase**: Converts to title case
- **Truncate**: Truncates string to specified length
- **IsValidEmail**: Validates email format
- **ToSlug**: Converts to slug format

## 🎯 Best Practices

1. **Use Pure Functions**: All utilities are pure functions with no side effects
2. **No Business Logic**: Only utility operations, no business rules
3. **Input Validation**: Always validate inputs before processing
4. **Error Handling**: Handle edge cases gracefully
5. **Performance**: Optimized for performance with compiled regex
6. **Reusability**: Designed for reuse across multiple services
7. **Consistency**: Consistent naming and behavior patterns

## 📄 License

This project is licensed under the MIT License.
