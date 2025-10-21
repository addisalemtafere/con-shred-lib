# Convex.Shared.Validation

Validation utilities for Convex microservices.

## Features

- **FluentValidation Integration**: Built on FluentValidation
- **Base Validators**: Common validation patterns
- **Email Validation**: Email format validation
- **Phone Validation**: Phone number format validation (International & Ethiopian)
- **Password Strength**: Password strength validation
- **Betting Validation**: Stake amounts, odds, bet slip IDs
- **Ethiopian Validation**: National ID, phone numbers, Birr amounts
- **Financial Validation**: Transaction references, account numbers, PINs
- **Age Validation**: Betting eligibility age verification
- **Custom Validators**: Easy to create custom validators

## Installation

```xml
<PackageReference Include="Convex.Shared.Validation" Version="1.0.0" />
```

## Quick Start

### 1. Register Services

```csharp
// In Program.cs
services.AddConvexValidation();
```

### 2. Create Validators

```csharp
public class UserValidator : BaseValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .Must(IsValidEmail).WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .Must(IsValidPhoneNumber).WithMessage("Invalid phone number format");

        RuleFor(x => x.Password)
            .Must(IsStrongPassword).WithMessage("Password must be strong");
    }
}
```

### 3. Use Validators

```csharp
public class UserService
{
    private readonly IValidator<User> _validator;

    public UserService(IValidator<User> validator)
    {
        _validator = validator;
    }

    public async Task<ApiResponse<User>> CreateUserAsync(User user)
    {
        var validationResult = await _validator.ValidateAsync(user);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
            return ApiResponse<User>.ErrorResult(errors);
        }

        // Create user...
        return ApiResponse<User>.SuccessResult(user);
    }
}
```

## Built-in Validators

### Email Validation
```csharp
RuleFor(x => x.Email)
    .Must(IsValidEmail).WithMessage("Invalid email format");
```

### Phone Validation
```csharp
RuleFor(x => x.Phone)
    .Must(IsValidPhoneNumber).WithMessage("Invalid phone number format");
```

### Password Strength
```csharp
RuleFor(x => x.Password)
    .Must(IsStrongPassword).WithMessage("Password must be strong");
```

### Ethiopian Phone Number
```csharp
RuleFor(x => x.PhoneNumber)
    .Must(IsValidEthiopianPhoneNumber).WithMessage("Invalid Ethiopian phone number");
```

### Betting Stake Amount
```csharp
RuleFor(x => x.StakeAmount)
    .Must(x => IsValidStakeAmount(x.StakeAmount, 1m, 100000m))
    .WithMessage("Stake amount must be between 1 and 100,000 ETB");
```

### Betting Odds
```csharp
RuleFor(x => x.Odds)
    .Must(x => IsValidOdds(x.Odds, 1.01m, 1000m))
    .WithMessage("Odds must be between 1.01 and 1000");
```

### Ethiopian National ID
```csharp
RuleFor(x => x.NationalId)
    .Must(IsValidEthiopianNationalId).WithMessage("Invalid Ethiopian national ID");
```

### Betting Age Eligibility
```csharp
RuleFor(x => x.BirthDate)
    .Must(IsEligibleBettingAge).WithMessage("Must be at least 18 years old to bet");
```

### Transaction Reference
```csharp
RuleFor(x => x.TransactionReference)
    .Must(IsValidTransactionReference).WithMessage("Invalid transaction reference format");
```

### Bet Slip ID
```csharp
RuleFor(x => x.BetSlipId)
    .Must(IsValidBetSlipId).WithMessage("Invalid bet slip ID format");
```

### PIN Validation
```csharp
RuleFor(x => x.Pin)
    .Must(IsValidPin).WithMessage("PIN must be 4-6 digits");
```

### Account Number
```csharp
RuleFor(x => x.AccountNumber)
    .Must(IsValidAccountNumber).WithMessage("Invalid account number format");
```

## Custom Validators

### Creating Custom Validators
```csharp
public class BetValidator : BaseValidator<Bet>
{
    public BetValidator()
    {
        RuleFor(x => x.StakeAmount)
            .Must(x => IsValidStakeAmount(x.StakeAmount, 1m, 100000m))
            .WithMessage("Stake amount must be between 1 and 100,000 ETB");

        RuleFor(x => x.Odds)
            .Must(x => IsValidOdds(x.Odds, 1.01m, 1000m))
            .WithMessage("Odds must be between 1.01 and 1000");

        RuleFor(x => x.BetSlipId)
            .NotEmpty().WithMessage("Bet slip ID is required")
            .Must(IsValidBetSlipId).WithMessage("Invalid bet slip ID format");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required")
            .Must(IsValidUserId).WithMessage("Invalid user ID format");
    }
}

public class UserRegistrationValidator : BaseValidator<UserRegistration>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Must(IsValidEthiopianPhoneNumber).WithMessage("Invalid Ethiopian phone number");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .Must(IsValidEmail).WithMessage("Invalid email format");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required")
            .Must(IsValidEthiopianNationalId).WithMessage("Invalid Ethiopian national ID");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required")
            .Must(IsEligibleBettingAge).WithMessage("Must be at least 18 years old to bet");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .Must(IsStrongPassword).WithMessage("Password must be strong");
    }
}

public class TransactionValidator : BaseValidator<Transaction>
{
    public TransactionValidator()
    {
        RuleFor(x => x.Amount)
            .Must(x => IsValidEthiopianBirrAmount(x.Amount, 0.01m, 1000000m))
            .WithMessage("Amount must be between 0.01 and 1,000,000 ETB");

        RuleFor(x => x.TransactionReference)
            .NotEmpty().WithMessage("Transaction reference is required")
            .Must(IsValidTransactionReference).WithMessage("Invalid transaction reference format");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .Must(IsValidAccountNumber).WithMessage("Invalid account number format");

        RuleFor(x => x.Pin)
            .NotEmpty().WithMessage("PIN is required")
            .Must(IsValidPin).WithMessage("PIN must be 4-6 digits");
    }
}
```

### Async Validators
```csharp
public class UserValidator : BaseValidator<User>
{
    private readonly IUserRepository _userRepository;

    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellation) =>
            {
                var existingUser = await _userRepository.GetByEmailAsync(email);
                return existingUser == null;
            }).WithMessage("Email already exists");
    }
}
```

## Validation Results

### Handling Validation Results
```csharp
var validationResult = await _validator.ValidateAsync(user);

if (!validationResult.IsValid)
{
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
```

### Converting to Dictionary
```csharp
var errors = validationResult.Errors
    .GroupBy(e => e.PropertyName)
    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
```

## Best Practices

1. **Use BaseValidator**: Inherit from BaseValidator for common patterns
2. **Async Validation**: Use async validators for database checks
3. **Clear Messages**: Provide clear, user-friendly error messages
4. **Reusable Rules**: Create reusable validation rules
5. **Performance**: Consider performance for complex validations

## License

This project is licensed under the MIT License.
