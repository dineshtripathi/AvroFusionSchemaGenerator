//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using AvroFusionGenerator.Implementation;
//using AvroFusionGenerator.Interface;
//using Microsoft.Extensions.DependencyInjection;

//namespace AvroFusionGenerator;

//public static class DependencyInjectionConfig
//{
//    public static IServiceProvider Configure(IServiceCollection services)
//    {
//        // Register services
       

//        // Configure Autofac
//        var builder = new ContainerBuilder();
//        RegisterTypeStrategies(services, builder);
//        builder.Populate(services);
//        var container = builder.Build();

//        // Use the Autofac container to create a service provider
//        return new AutofacServiceProvider(container);
//    }

//    private static void RegisterTypeStrategies(IServiceCollection services, ContainerBuilder builder)
//    {
//        // Register types implementing IAvroTypeStrategy from the current assembly
//        builder.RegisterAssemblyTypes(typeof(global::AvroFusionGenerator.AvroFusionGenerator).Assembly)
//            .Where(t => typeof(IAvroTypeStrategy).IsAssignableFrom(t))
//            .AsImplementedInterfaces();
//    }
//}