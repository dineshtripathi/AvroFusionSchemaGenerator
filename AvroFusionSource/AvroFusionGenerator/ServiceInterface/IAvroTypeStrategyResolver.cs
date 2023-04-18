namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroTypeStrategyResolver
{
    IEnumerable<IAvroAvscTypeHandler> ResolveStrategies();
}