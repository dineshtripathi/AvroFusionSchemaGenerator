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
        private static IServiceCollection services;
        static SpectreServiceProviderTypeRegistrar typeRegistrar;
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
            services = new ServiceCollection();
            services = ConfigureServices(services);

            typeRegistrar = new SpectreServiceProviderTypeRegistrar(services);
          
            var app = new CommandApp(typeRegistrar);

            app.Configure(config =>
            {
                config.AddCommand<GenerateCommand>("generate");
            });

            return await app.RunAsync(args);

        }
    }
}