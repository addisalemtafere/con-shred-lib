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

## Dependencies
- Microsoft.Extensions.Logging.Abstractions
