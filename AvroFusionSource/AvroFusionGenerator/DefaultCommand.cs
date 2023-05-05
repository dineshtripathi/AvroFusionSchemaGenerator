using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator;
/// <summary>
/// The default command.
/// </summary>

public class DefaultCommand : SpectreGenerateCommand
{
    public DefaultCommand(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
        : base(avroSchemaGenerator, compilerService)
    {
    }
}