using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator.DIRegistration;
/// <summary>
/// The command builder registration.
/// </summary>

public interface IAvroFusionCommandBuilderRegistration
{
    /// <summary>
    /// Registers the command builder.
    /// </summary>
    /// <param name="services">The services.</param>
    public void RegisterCommandBuilder(IServiceCollection? services);
}