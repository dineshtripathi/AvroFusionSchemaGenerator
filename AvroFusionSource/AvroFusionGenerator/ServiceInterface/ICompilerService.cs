namespace AvroFusionGenerator.ServiceInterface;

public interface ICompilerService
{
    List<Type> LoadTypesFromSource(string sourceCode);
}