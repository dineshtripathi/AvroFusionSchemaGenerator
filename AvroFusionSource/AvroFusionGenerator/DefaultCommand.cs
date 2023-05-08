using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator;
/// <summary>
/// The default command.
/// </summary>

public class DefaultCommand : SpectreGenerateCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCommand"/> class.
    /// </summary>
    /// <param name="avroFusionSchemaGenerator">The avro fusion schema generator.</param>
    /// <param name="avroFusionCompilerService">The avro fusion compiler service.</param>
    public DefaultCommand(IAvroFusionSchemaGenerator avroFusionSchemaGenerator, IAvroFusionCompilerService avroFusionCompilerService)
        : base(avroFusionSchemaGenerator, avroFusionCompilerService)
    {
    }
}