using AvroFusionGenerator.Implementation;

namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroSchemaGenerator
{
    string GenerateCombinedSchema(IEnumerable<Type> types, string mainClassName, ProgressReporter progressReporter);
}