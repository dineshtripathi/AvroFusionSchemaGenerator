using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace AvroFusionGeneratorReadMeUpdater
{

    namespace UpdateReadme
    {
        class Program
        {
            static async Task Main(string[] args)
            {
                string token = Environment.GetEnvironmentVariable("AVROFUSIONGENERATOR_TOKEN");

                var serviceProvider = new ServiceCollection()
                    .AddTransient<IReadmeUpdater, ReadmeUpdater>()
                    .AddTransient<IGitHubService>(provider => new GitHubService(token))
                    .BuildServiceProvider();

                var readmeUpdater = serviceProvider.GetService<IReadmeUpdater>();
                var gitHubService = serviceProvider.GetService<IGitHubService>();

                var (packageVersion, tag) = await gitHubService.GetPackageVersionAndTagAsync();
                readmeUpdater.UpdateReadmeFile(packageVersion, tag);
            }
        }
    }

}