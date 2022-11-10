namespace Olbrasoft.Data.Cqrs.FreeSql;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Get service of type <paramref name="exactTypeConfiguration"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
    /// <param name="exactTypeConfiguration">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="exactTypeConfiguration"/>.</returns>
    /// <exception cref="InvalidOperationException">There is no service of type <paramref name="exactTypeConfiguration"/>.</exception>
    public static IConfiguration GetConfiguration(this IServiceProvider provider, Type exactTypeConfiguration)
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        if (exactTypeConfiguration == null)
        {
            throw new ArgumentNullException(nameof(exactTypeConfiguration));
        }

        if (provider is ISupportRequiredService requiredServiceSupportingProvider)
        {
            return (IConfiguration)requiredServiceSupportingProvider.GetRequiredService(exactTypeConfiguration);
        }

        object? service = provider.GetService(exactTypeConfiguration);

        return service is null ? throw new InvalidOperationException(nameof(exactTypeConfiguration) + " not found") : (IConfiguration)service;
    }

    public static bool TryGetConfiguration(this IServiceProvider provider, Type exactTypeConfiguration, out IConfiguration? configuration)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        if (exactTypeConfiguration is null) throw new ArgumentNullException(nameof(exactTypeConfiguration));

        configuration = (IConfiguration?)provider.GetService(exactTypeConfiguration);

        if (configuration is null) return false;

        return true;
    }

}