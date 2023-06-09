﻿using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AvroFusionGenerator;
/// <summary>
/// The spectre service provider type resolver.
/// </summary>

public class SpectreServiceProviderTypeResolver : ITypeResolver
{
    private readonly IServiceProvider? _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreServiceProviderTypeResolver"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public SpectreServiceProviderTypeResolver(IServiceProvider? serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Resolves the.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>An object.</returns>
    public object? Resolve(Type? type)
    {
        return type != null ? _serviceProvider?.GetRequiredService(type) : null;
    }
}