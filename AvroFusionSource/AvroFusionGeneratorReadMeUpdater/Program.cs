using Microsoft.Extensions.DependencyInjection;

var token = Environment.GetEnvironmentVariable("BUILD_TAGGING_ACCESS_TOKEN");
Console.WriteLine($"GITHUB ACCESS TOKEN :{token}");
var serviceProvider = new ServiceCollection()
    .AddTransient<IReadmeUpdater, ReadmeUpdater>()
    .AddTransient<IGitHubService>(provider => new GitHubService(token))
    .BuildServiceProvider();

var readmeUpdater = serviceProvider.GetService<IReadmeUpdater>();
var gitHubService = serviceProvider.GetService<IGitHubService>();

var (packageVersion, packageName,releaseNumber) = await gitHubService?.GetPackageVersionAndTagAsync();
readmeUpdater?.UpdateReadmeFile(packageName,packageVersion,releaseNumber);

