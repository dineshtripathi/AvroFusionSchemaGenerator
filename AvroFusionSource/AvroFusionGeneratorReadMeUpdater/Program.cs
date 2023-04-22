using Microsoft.Extensions.DependencyInjection;

var token = Environment.GetEnvironmentVariable("AVROFUSIONGENERATOR_TOKEN");

var serviceProvider = new ServiceCollection()
    .AddTransient<IReadmeUpdater, ReadmeUpdater>()
    .AddTransient<IGitHubService>(provider => new GitHubService(token))
    .BuildServiceProvider();

var readmeUpdater = serviceProvider.GetService<IReadmeUpdater>();
var gitHubService = serviceProvider.GetService<IGitHubService>();

var (packageVersion, tag) = await gitHubService?.GetPackageVersionAndTagAsync();
var packageName = gitHubService.GetPackageName();

readmeUpdater?.UpdateReadmeFile(packageName, packageVersion, tag);

