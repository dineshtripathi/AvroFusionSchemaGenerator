using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator;
/// <summary>
/// The default command.
/// </summary>

public class DefaultCommand : SpectreGenerateCommand
{
    public DefaultCommand(IAvroFusionSchemaGenerator avroFusionSchemaGenerator, IAvroFusionCompilerService avroFusionCompilerService)
        : base(avroFusionSchemaGenerator, avroFusionCompilerService)
    {
    }
}