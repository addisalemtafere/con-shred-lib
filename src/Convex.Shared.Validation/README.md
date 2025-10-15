# Convex.Shared.Validation

Validation utilities for Convex microservices.

## Features

- **FluentValidation Integration**: Built on FluentValidation
- **Base Validators**: Common validation patterns
- **Email Validation**: Email format validation
- **Phone Validation**: Phone number format validation
- **Password Strength**: Password strength validation
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

## Custom Validators

### Creating Custom Validators
```csharp
public class BetValidator : BaseValidator<Bet>
{
    public BetValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .LessThan(10000).WithMessage("Amount cannot exceed $10,000");

        RuleFor(x => x.Odds)
            .GreaterThan(1).WithMessage("Odds must be greater than 1")
            .LessThan(1000).WithMessage("Odds cannot exceed 1000");

        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event ID is required");
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
