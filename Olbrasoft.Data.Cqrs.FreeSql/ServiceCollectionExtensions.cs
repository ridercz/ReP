using Microsoft.Extensions.DependencyInjection.Extensions;
using Olbrasoft.Data.Cqrs.FreeSql;
using Olbrasoft.Extensions;
using System.Reflection;

namespace Olbrasoft.Data.Cqrs.FreeSql;
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Registers Configurator and Configurations types from the specified assemblies
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assemblies">Assemblies to scan</param>        
    /// <returns>Service collection</returns>
    public static IServiceCollection AddProjectionConfigurations(this IServiceCollection services, params Assembly[] assemblies)
    {

        services.AddScoped(typeof(IConfigure<>), typeof(ProjectionConfigurator<>));

        services.TryAddScoped<TryCreateConfiguration>(p => p.TryGetConfiguration);

        foreach (var typeInfo in ConfigurationTypes(assemblies))
        {
            services.TryAddScoped(typeInfo.GetInterfaces().First(), typeInfo);
        }

        return services;
    }

    public static IEnumerable<TypeInfo> ConfigurationTypes(IEnumerable<Assembly> assemblies)
    {
        if (assemblies is null)
            throw new ArgumentNullException(nameof(assemblies));

        var configurationsGenericInterfaceType = new[] { typeof(IEntityToDtoConfigure<,>) };

        return configurationsGenericInterfaceType.SelectMany(openType => AllTypes(assemblies)
             .Where(t => t.AsType().ImplementsGenericInterface(openType) && !t.IsGenericType));
    }

    private static IEnumerable<TypeInfo> AllTypes(IEnumerable<Assembly> assemblies)
    {
        return assemblies.Where(a => !a.IsDynamic && a.GetName().Name != typeof(IRequest<>).Assembly.GetName().Name).Distinct()
            .SelectMany(a => a.DefinedTypes).Where(c => c.IsClass && !c.IsAbstract);
    }
}
