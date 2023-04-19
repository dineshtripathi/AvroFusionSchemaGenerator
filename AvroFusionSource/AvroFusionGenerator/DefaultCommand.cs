using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator;
/// <summary>
/// The default command.
/// </summary>

public class DefaultCommand : GenerateCommand
{
    public DefaultCommand(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
        : base(avroSchemaGenerator, compilerService)
    {
    }
}