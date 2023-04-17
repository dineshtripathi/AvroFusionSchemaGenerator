using Autofac;
using Autofac.Extensions.DependencyInjection;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;

public class TypeStrategyRegistration : ITypeStrategyRegistration
{
    public void RegisterTypeStrategies(ContainerBuilder builder)
    {
        // Get all types from the current assembly
        var allTypes = typeof(AvroFusionGenerator).Assembly.GetTypes();
        Console.WriteLine("All types in assembly:");
        foreach (var type in allTypes)
        {
            Console.WriteLine($" - {type.FullName}");
        }

        // Filter types implementing IAvroTypeStrategy
        var typeStrategyTypes = allTypes.Where(t => typeof(IAvroTypeStrategy).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).ToList();
        Console.WriteLine("Types implementing IAvroTypeStrategy:");
        foreach (var type in typeStrategyTypes)
        {
            Console.WriteLine($" - {type.FullName}");
        }
        

        // Register types implementing IAvroTypeStrategy from the current assembly
        builder.RegisterAssemblyTypes(typeof(AvroFusionGenerator).Assembly)
            .Where(t => typeof(IAvroTypeStrategy).IsAssignableFrom(t))
            .AsImplementedInterfaces().InstancePerDependency();


    }

    public void RegisterTypeStrategies(IServiceCollection services)
    {
        services?.AddSingleton<IAvroTypeStrategy, AvroBooleanStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroByteStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroCharStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroDateTimeOffsetStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroDateTimeStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroDecimalStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroDictionaryTypeStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroDoubleStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroEnumTypeStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroGuidStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroByteStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroInt16Strategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroInt32Strategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroInt64Strategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroListTypeStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroUInt16Strategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroUInt32Strategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroUInt64Strategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroStringStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroSByteStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroSingleStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroClassTypeStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroCharStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroNullableTypeStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroTimseSpanStrategy>();
        services?.AddSingleton<IAvroTypeStrategy, AvroEqualityComparerTypeStrategy>();



    }
}