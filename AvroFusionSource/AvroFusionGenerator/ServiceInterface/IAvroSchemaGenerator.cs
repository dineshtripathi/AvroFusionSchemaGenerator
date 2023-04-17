using AvroFusionGenerator.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroSchemaGenerator
{
    string GenerateCombinedSchema(IEnumerable<Type> types, string mainClassName, ProgressReporter progressReporter);
    object GenerateAvroType(Type type, HashSet<string> generatedTypes);
}

public interface IAvroTypeStrategyResolver
{
    IEnumerable<IAvroTypeStrategy> ResolveStrategies();
}


public class AvroTypeStrategyResolver : IAvroTypeStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public AvroTypeStrategyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<IAvroTypeStrategy> ResolveStrategies()
    {
        return _serviceProvider.GetServices<IAvroTypeStrategy>();
    }
}
