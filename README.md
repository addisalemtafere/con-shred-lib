# Convex Shared Libraries

A comprehensive collection of .NET 9.0 shared libraries for the Convex betting platform, designed for microservices architecture with zero dependencies between libraries.

## 🏗️ Architecture

This project consists of 9 independent shared libraries, each designed to be deployed and used independently:

### Core Libraries

#### 1. **Convex.Shared.Common** 
- **Purpose**: Common utilities and business logic
- **Features**: 
  - Business calculation utilities (GGR, tax, cashout)
  - Hash generation and verification
  - Correlation ID management
  - Extension methods
- **Dependencies**: None (completely independent)

#### 2. **Convex.Shared.Models**
- **Purpose**: Data models, enums, and constants
- **Features**:
  - Betting status enums (BettingStatus, WinStatus, MatchStatus)
  - User type enums (UserType, PaymentStatus, PaymentType)
  - Jackpot enums (JackpotChoice, JackpotWonStatus)
  - Comprehensive permission constants (100+ permissions)
  - Financial constants (betting limits, tax rates, fees)
  - Environment configuration constants
- **Dependencies**: None (completely independent)

### Infrastructure Libraries

#### 3. **Convex.Shared.Logging**
- **Purpose**: Structured logging with Serilog
- **Features**:
  - Enhanced logging with correlation ID support
  - Performance logging
  - Business event logging
  - API request/response logging
- **Dependencies**: None (completely independent)

#### 4. **Convex.Shared.Messaging**
- **Purpose**: Apache Kafka message bus
- **Features**:
  - Kafka producer/consumer
  - Message publishing and subscription
  - Topic management
- **Dependencies**: None (completely independent)

#### 5. **Convex.Shared.Http**
- **Purpose**: HTTP client utilities
- **Features**:
  - Custom HTTP client with retry logic
  - Request/response logging
  - Timeout configuration
- **Dependencies**: None (completely independent)

#### 6. **Convex.Shared.Caching**
- **Purpose**: Redis caching utilities
- **Features**:
  - Redis connection management
  - Cache operations (get, set, delete)
  - Cache key generation
- **Dependencies**: None (completely independent)

#### 7. **Convex.Shared.Security**
- **Purpose**: Security and authentication
- **Features**:
  - JWT token validation
  - Authentication middleware
  - Security headers
- **Dependencies**: None (completely independent)

#### 8. **Convex.Shared.Validation**
- **Purpose**: Data validation utilities
- **Features**:
  - Fluent validation
  - Custom validators
  - Validation error handling
- **Dependencies**: None (completely independent)

#### 9. **Convex.Shared.Utilities**
- **Purpose**: General utility functions
- **Features**:
  - String utilities
  - Cache helpers
  - Extension methods
- **Dependencies**: None (completely independent)

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Docker (for development dependencies)

### Development Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd new-sport-book
   ```

2. **Start development dependencies**
   ```bash
   docker-compose up -d
   ```
   This starts:
   - Redis (port 6379)
   - Apache Kafka (port 9092)
   - Kafka UI (port 8080)
   - Seq (port 5341)

3. **Build the solution**
   ```bash
   dotnet build Convex.Shared.sln --configuration Release
   ```

4. **Run tests** (if available)
   ```bash
   dotnet test
   ```

### Package Management

The project uses **Central Package Management** with `Directory.Packages.props`:

```xml
<PackageVersion Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
<PackageVersion Include="Serilog" Version="4.0.0" />
<PackageVersion Include="Confluent.Kafka" Version="2.3.0" />
<!-- ... more packages -->
```

## 📦 NuGet Packages

Each library is packaged as a NuGet package:

- `Convex.Shared.Common.1.0.0.nupkg`
- `Convex.Shared.Models.1.0.0.nupkg`
- `Convex.Shared.Logging.1.0.0.nupkg`
- `Convex.Shared.Messaging.1.0.0.nupkg`
- `Convex.Shared.Http.1.0.0.nupkg`
- `Convex.Shared.Caching.1.0.0.nupkg`
- `Convex.Shared.Security.1.0.0.nupkg`
- `Convex.Shared.Validation.1.0.0.nupkg`
- `Convex.Shared.Utilities.1.0.0.nupkg`

## 🔧 Usage Examples

### Business Logic (Common Library)
```csharp
using Convex.Shared.Common.Business;

// Calculate possible win
var possibleWin = BettingCalculator.CalculatePossibleWin(stake, totalOdds);

// Check cashout eligibility
var isEligible = BettingCalculator.IsEligibleForCashout(stake, totalOdds, matchCount);

// Generate unique slip ID
var slipId = HashHelper.GenerateBettingSlipId(stake, gamePickIds, userId, DateTime.UtcNow);
```

### Logging
```csharp
using Convex.Shared.Logging;

// Register logging services
services.AddConvexLogging();

// Use in your service
public class MyService
{
    private readonly IConvexLogger _logger;
    
    public MyService(IConvexLogger logger)
    {
        _logger = logger;
    }
    
    public async Task DoSomething()
    {
        _logger.LogInformation("Processing request");
        _logger.LogPerformance("Operation completed", TimeSpan.FromMilliseconds(100));
    }
}
```

### Messaging
```csharp
using Convex.Shared.Messaging;

// Register messaging services
services.AddConvexMessaging();

// Use in your service
public class MyService
{
    private readonly IConvexMessageBus _messageBus;
    
    public async Task PublishEvent()
    {
        await _messageBus.PublishAsync("my-topic", "Hello World");
    }
}
```

### HTTP Client
```csharp
using Convex.Shared.Http;

// Register HTTP client
services.AddConvexHttpClient();

// Use in your service
public class MyService
{
    private readonly ConvexHttpClient _httpClient;
    
    public async Task CallApi()
    {
        var response = await _httpClient.GetAsync("https://api.example.com/data");
  }
}
```

## 🏗️ Migration from Django

This project represents a complete migration from a Django monolithic application to a .NET microservices architecture:

### Migrated Components
- ✅ **All Constants** - 200+ constants from Django
- ✅ **All Enums** - 20+ enums with type safety
- ✅ **All Business Logic** - GGR calculation, tax calculation, hash generation
- ✅ **All Permissions** - 100+ permission codes
- ✅ **All Financial Logic** - Betting limits, fees, cashout rules

### Benefits of Migration
- **Type Safety** - Strong typing with enums instead of magic strings
- **Performance** - Optimized .NET implementations
- **Maintainability** - Clean separation of concerns
- **Scalability** - Independent microservices
- **Developer Experience** - IntelliSense and compile-time validation

## 🧪 Development

### Building
```bash
# Build all libraries
dotnet build Convex.Shared.sln --configuration Release

# Build specific library
dotnet build src/Convex.Shared.Common/Convex.Shared.Common.csproj
```

### Publishing
```bash
# Publish all libraries
dotnet publish Convex.Shared.sln --configuration Release

# Publish specific library
dotnet publish src/Convex.Shared.Common/Convex.Shared.Common.csproj --configuration Release
```

### Package Creation
```bash
# Create NuGet packages
dotnet pack Convex.Shared.sln --configuration Release
```

## 📁 Project Structure

```
src/
├── Convex.Shared.Common/           # Common utilities and business logic
├── Convex.Shared.Models/           # Data models, enums, constants
├── Convex.Shared.Logging/          # Structured logging
├── Convex.Shared.Messaging/       # Apache Kafka messaging
├── Convex.Shared.Http/            # HTTP client utilities
├── Convex.Shared.Caching/         # Redis caching
├── Convex.Shared.Security/         # Security and authentication
├── Convex.Shared.Validation/      # Data validation
└── Convex.Shared.Utilities/       # General utilities
```

## 🔒 Security

- All libraries follow security best practices
- No hardcoded secrets or credentials
- Secure by default configurations
- Input validation and sanitization

## 📈 Performance

- Optimized for high-performance scenarios
- Minimal memory allocations
- Efficient serialization/deserialization
- Async/await patterns throughout

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation

---

**Built with ❤️ by the Convex Team**