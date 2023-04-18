using AvroFusionGenerator.Implementation;

namespace AvroFusionGenerator.ServiceInterface;

public interface IAvroSchemaGenerator
{
    string GenerateAvroAvscSshema(IEnumerable<Type> types, string mainClassName, ProgressReporter progressReporter);
    object GenerateAvroAvscType(Type type, HashSet<string> generatedTypes);
}