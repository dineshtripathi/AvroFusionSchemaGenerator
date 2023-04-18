using System.CommandLine;
using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Spectre.Console;
using Autofac.Core;

namespace AvroFusionGenerator
{
    public class AvroFusionGenerator : AvroFusionGeneratorBase
    {
        private static IServiceCollection? _services;
        static SpectreServiceProviderTypeRegistrar _typeRegistrar;
        public AvroFusionGenerator()
            : base()
        {
        }

        public static async Task<int> Main(string[] args)
        {

            var program = new AvroFusionGenerator();
            return await program.Run(args);
        }

        public async Task<int> Run(string[] args)
        {
            _services = new ServiceCollection();
            _services = ConfigureServices(_services);

            _typeRegistrar = new SpectreServiceProviderTypeRegistrar(_services);
          
            var app = new CommandApp(_typeRegistrar);

            app.Configure(config =>
            {
                config.AddCommand<GenerateCommand>("generate");
            });

            return await app.RunAsync(args);

        }
    }
}