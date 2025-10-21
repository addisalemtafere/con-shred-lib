# Convex.Shared.Business

## Overview
Business logic and calculations for Convex microservices following SOLID principles.

## Features
- **IBettingCalculator**: Interface for betting calculations
- **BettingCalculator**: Service implementation with dependency injection
- **BettingConfiguration**: Configurable business rules
- **Service Registration**: Easy DI setup

## SOLID Principles
- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Extensible through configuration
- **Liskov Substitution**: Proper interface implementation
- **Interface Segregation**: Focused interfaces
- **Dependency Inversion**: Depends on abstractions

## SOAR Compliance
- **Scalable**: Configurable business rules
- **Observable**: Comprehensive logging
- **Available**: Fault-tolerant error handling
- **Reliable**: Input validation and error handling

## Usage
```csharp
// Register services
services.AddBusinessServices();

// Or with custom configuration
services.AddBusinessServices(config =>
{
    config.TaxableWinThreshold = 2000m;
    config.WinTaxRate = 0.20m;
});

// Use in your service
public class MyService
{
    private readonly IBettingCalculator _calculator;
    
    public MyService(IBettingCalculator calculator)
    {
        _calculator = calculator;
    }
    
    public decimal CalculateWin(decimal stake, decimal odds)
    {
        return _calculator.CalculatePossibleWin(stake, odds);
    }
}
```

## Exception Handling

### Input Validation
```csharp
// ✅ Library validates inputs and throws specific exceptions
try
{
    var win = _calculator.CalculatePossibleWin(-100, 2.5m); // Throws ArgumentException
}
catch (ArgumentException ex)
{
    // Handle invalid input
    Console.WriteLine($"Invalid input: {ex.Message}");
}
```

### Business Logic Validation
```csharp
// ✅ Business rules are enforced with exceptions
try
{
    var tax = _calculator.CalculateTax(winAmount);
    var winValue = _calculator.CalculateWinValue(stake, odds, 0); // Throws ArgumentException
}
catch (ArgumentException ex)
{
    // Handle business rule violations
    _logger.LogWarning("Business rule violation: {Message}", ex.Message);
}
```

### Best Practice: Application-Level Error Handling
```csharp
public async Task<decimal> ProcessBetAsync(decimal stake, decimal odds)
{
    try
    {
        return await _calculator.CalculatePossibleWin(stake, odds);
    }
    catch (ArgumentException ex)
    {
        // Log and handle business rule violations
        _logger.LogWarning("Invalid bet parameters: {Message}", ex.Message);
        throw new InvalidBetException("Invalid bet parameters", ex);
    }
}
```

## Performance Features

### High-Performance Design
- **Pure Business Logic**: No external dependencies in calculations
- **Efficient Algorithms**: Optimized mathematical operations
- **Memory Efficient**: Minimal object allocation
- **Thread Safe**: Stateless service design
- **Configurable**: Runtime configuration without recompilation

### Exception Handling Best Practices
- **Input Validation**: Comprehensive parameter validation with specific exceptions
- **Exception Propagation**: Let exceptions bubble up naturally
- **No Exception Swallowing**: Clear error information for debugging
- **Business Rules**: Enforced through validation and exceptions

## Dependencies
- Microsoft.Extensions.Logging.Abstractions
