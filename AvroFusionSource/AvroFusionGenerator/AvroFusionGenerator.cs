using System.CommandLine;
using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace AvroFusionGenerator
{
    public class AvroFusionGenerator : AvroFusionGeneratorBase
    {
        public AvroFusionGenerator(IServiceProvider? services)
            : base(services)
        {
        }

        public static async Task<int> Main(string[] args)
        {
            var services = ConfigureServices()?.BuildServiceProvider();
            var commandBuilder = services?.GetService<CommandBuilder>();

            var program = new AvroFusionGenerator(services);
            return await program.Run(args);
        }

        public async Task<int> Run(string[] args)
        {
            var serviceProvider = Services.BuildServiceProvider();

            var commandBuilder = serviceProvider.GetService<CommandBuilder>();
            var rootCommand = commandBuilder?.BuildRootCommand();
            rootCommand.Description = "AvroSchemaGenerator - Generate Avro Schemas from C# types.";

            var generateCommandHandler = serviceProvider.GetService<GenerateCommandHandler>();
            rootCommand.Handler = generateCommandHandler;

            return await rootCommand.InvokeAsync(args);
        }
    }
}