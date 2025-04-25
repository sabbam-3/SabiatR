using Microsoft.Extensions.DependencyInjection;
using SabiatR.Configurations;

namespace SabiatR;

public static class DependencyInjection
{
    private static Type RequestHandlerType = typeof(IRequestHandler<>);
    private static Type RequestHandlerReturningType = typeof(IRequestHandler<,>);

    private static Type PipelineBehaviourType = typeof(IPipelineBehavior<>);
    private static Type PipelineBehaviourReturningType = typeof(IPipelineBehavior<,>);

    public static IServiceCollection AddSabiatR(this IServiceCollection services, Action<SabiatRConfiguration> configure, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        SabiatRConfiguration configuration = new SabiatRConfiguration();
        configure(configuration);

        services.AddTransient<ISender, Sender>();

        foreach (var assembly in configuration.Assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract);

            // Register request handlers
            foreach (var type in types)
            {
                foreach (var iface in type.GetInterfaces()
                             .Where(i => i.IsGenericType &&
                                 (i.GetGenericTypeDefinition() == RequestHandlerType ||
                                  i.GetGenericTypeDefinition() == RequestHandlerReturningType)))
                {
                    services.Add(new ServiceDescriptor(iface, type, lifetime));
                }
            }
        }

        // Register pipelines
        foreach (var pipeline in configuration.PipelineBehaviors)
        {
            foreach (var iface in pipeline.GetInterfaces()
                .Where(i => i.IsGenericType &&
                       (i.GetGenericTypeDefinition() == PipelineBehaviourType ||
                        i.GetGenericTypeDefinition() == PipelineBehaviourReturningType)))
            {
                // Check if the implementation is open generic
                if (pipeline.IsGenericTypeDefinition)
                {
                    services.Add(new ServiceDescriptor(iface.GetGenericTypeDefinition(), pipeline, lifetime));
                }
                else
                {
                    services.Add(new ServiceDescriptor(iface, pipeline, lifetime));
                }
            }
        }

        return services;
    }
}
