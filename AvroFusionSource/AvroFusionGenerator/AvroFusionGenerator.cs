using System.CommandLine;
using AvroFusionGenerator.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Spectre.Console;
using Autofac.Core;
using AvroFusionGenerator.ServiceInterface;

namespace AvroFusionGenerator
{
    public class AvroFusionGenerator : AvroFusionGeneratorBase
    {
        private static IServiceCollection services;
        public AvroFusionGenerator(IServiceProvider? services)
            : base(services)
        {
        }

        public static async Task<int> Main(string[] args)
        {
            services=ConfigureServices();
            var commandBuilder =GetService<CommandBuilder>();

            var program = new AvroFusionGenerator(FusionGeneratorServiceProvider);
            return await program.Run(args);
        }

        public async Task<int> Run(string[] args)
        {
            //  var serviceProvider = Services.BuildServiceProvider();

            //var commandBuilder = GetService<CommandBuilder>();
            //var rootCommand = commandBuilder?.BuildRootCommand();
            //rootCommand.Description = "AvroSchemaGenerator - Generate Avro Schemas from C# types.";

            //var generateCommandHandler = GetService<GenerateCommandHandler>();
            //rootCommand.Handler = generateCommandHandler;

            //return await rootCommand.InvokeAsync(args);
            ConfigureServices();

            var typeRegistrar = new SpectreServiceProviderTypeRegistrar(services);
            var app = new CommandApp(typeRegistrar);

            app.Configure(config =>
            {
                config.AddCommand<GenerateCommand>("generate");
              //  config.AddCommand<DefaultCommand>("");
            });

            return await app.RunAsync(args);

        }
    }

    public class SpectreServiceProviderTypeResolver : ITypeResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public SpectreServiceProviderTypeResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Resolve(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }
    }


    public class SpectreServiceProviderTypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _services;

        public SpectreServiceProviderTypeRegistrar(IServiceCollection services)
        {
            _services = services;
        }

        public void Register(Type service, Type implementation)
        {
            _services.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _services.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _services.AddSingleton(service, factory);
        }

        public ITypeResolver Build()
        {
            var serviceProvider = _services.BuildServiceProvider();
            return new SpectreServiceProviderTypeResolver(serviceProvider);
        }
    }
    public class DefaultCommand : GenerateCommand
    {
        public DefaultCommand(IAvroSchemaGenerator avroSchemaGenerator, ICompilerService compilerService)
            : base(avroSchemaGenerator, compilerService)
        {
        }
    }

}