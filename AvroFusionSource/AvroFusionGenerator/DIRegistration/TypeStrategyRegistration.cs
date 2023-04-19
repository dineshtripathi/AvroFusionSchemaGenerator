using Autofac;
using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;
/// <summary>
/// The type strategy registration.
/// </summary>

public class TypeStrategyRegistration : ITypeStrategyRegistration
{
    /// <summary>
    /// Registers the type strategies.
    /// </summary>
    /// <param name="builder">The builder.</param>
    public void RegisterTypeStrategies(ContainerBuilder builder)
    {
        // Get all types from the current assembly
        var allTypes = typeof(AvroFusionGenerator).Assembly.GetTypes();
        Console.WriteLine("All types in assembly:");
        foreach (var type in allTypes) Console.WriteLine($" - {type.FullName}");

        // Filter types implementing IAvroAvscTypeHandler
        var typeStrategyTypes = allTypes
            .Where(t => typeof(IAvroAvscTypeHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).ToList();
        Console.WriteLine("Types implementing IAvroAvscTypeHandler:");
        foreach (var type in typeStrategyTypes) Console.WriteLine($" - {type.FullName}");


        // Register types implementing IAvroAvscTypeHandler from the current assembly
        builder.RegisterAssemblyTypes(typeof(AvroFusionGenerator).Assembly)
            .Where(t => typeof(IAvroAvscTypeHandler).IsAssignableFrom(t))
            .AsImplementedInterfaces().InstancePerDependency();
    }

    /// <summary>
    /// Registers the type strategies.
    /// </summary>
    /// <param name="services">The services.</param>
    public void RegisterTypeStrategies(IServiceCollection services)
    {
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscBooleanHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscByteHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscCharHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscDateTimeOffsetHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscDateTimeHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscDecimalHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscDictionaryTypeHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscDoubleHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscEnumTypeHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscGuidHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscByteHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscInt16Handler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscInt32Handler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscInt64Handler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscListTypeHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscUInt16Handler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscUInt32Handler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscUInt64Handler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscStringHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscSByteHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscSingleHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscClassTypeHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscCharHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscNullableTypeHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscTimseSpanHandler>();
        services?.AddSingleton<IAvroAvscTypeHandler, AvroAvscEqualityComparerTypeHandler>();
    }
}