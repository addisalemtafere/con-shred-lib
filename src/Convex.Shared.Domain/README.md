# Convex.Shared.Domain

## Overview
Domain models and base classes for Convex microservices following Domain-Driven Design (DDD) principles.

## Features
- **Entity<TId>**: Base entity class with identity and equality
- **AggregateRoot<TId>**: Base aggregate root with domain events
- **DomainEvent**: Base domain event class with correlation tracking

## SOLID Principles
- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Extensible through inheritance
- **Liskov Substitution**: Proper inheritance hierarchy
- **Interface Segregation**: Focused interfaces
- **Dependency Inversion**: Depends on abstractions

## SOAR Compliance
- **Scalable**: Generic design supports any ID type
- **Observable**: Built-in correlation ID tracking
- **Available**: Thread-safe and reliable
- **Reliable**: Proper equality and hash code implementation

## Usage
```csharp
// Create a domain entity
public class User : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// Create an aggregate root
public class Order : AggregateRoot<Guid>
{
    public decimal Amount { get; set; }
    
    public void AddItem(string item)
    {
        // Business logic here
        AddDomainEvent(new OrderItemAddedEvent(Id, item));
    }
}
```

## Dependencies
- MediatR (for domain events)
