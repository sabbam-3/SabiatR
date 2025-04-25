using System.Reflection;

namespace SabiatR.Configurations;

public class SabiatRConfiguration
{
    internal List<Assembly> Assemblies { get; } = new();
    internal List<Type> PipelineBehaviors { get; } = new();

    public void RegisterAssembly(params Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
    }

    public void RegisterAssemblies(Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
    }

    public void AddPipelineBehaviour(params Type[] pipelineBehaviors)
    {
        PipelineBehaviors.AddRange(pipelineBehaviors);
    }

    public void AddPipelineBehaviours(Type[] pipelineBehaviors)
    {
        PipelineBehaviors.AddRange(pipelineBehaviors);
    }
}
