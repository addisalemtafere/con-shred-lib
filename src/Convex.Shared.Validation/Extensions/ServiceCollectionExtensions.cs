using Convex.Shared.Validation.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Convex.Shared.Validation.Extensions;

/// <summary>
/// Extension methods for configuring Convex validation services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Convex validation services
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddConvexValidation(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<BaseValidator<object>>();

        return services;
    }
}