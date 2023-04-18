using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator;

public class DefaultCommand : GenerateCommand
{
    public DefaultCommand(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
        : base(avroSchemaGenerator, compilerService)
    {
    }
}