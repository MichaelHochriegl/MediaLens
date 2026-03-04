using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediaLens.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring and registering services related to MediaLens
/// in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the MediaLens service to the provided <see cref="IServiceCollection"/> with the default configuration settings.
    /// </summary>
    /// <remarks>
    /// This method registers the MediaLens service with a singleton lifetime.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the MediaLens service is added.</param>
    /// <returns>The original <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddMediaLens(this IServiceCollection services)
        => services.AddMediaLens(ServiceLifetime.Singleton);

    /// <summary>
    /// Registers the MediaLens service implementation and its associated abstraction in the service collection
    /// with the specified lifetime.
    /// </summary>
    /// <param name="services">The service collection to which the MediaLens service will be added.</param>
    /// <param name="lifetime">The lifetime of the MediaLens service (e.g., Singleton, Scoped, or Transient).</param>
    /// <returns>The updated service collection with the MediaLens service registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="services"/> parameter is null.</exception>
    public static IServiceCollection AddMediaLens(this IServiceCollection services, ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(services);

        var descriptor = new ServiceDescriptor(typeof(IMediaLens), typeof(MediaLens), lifetime);

        services.TryAdd(descriptor);

        return services;
    }
}