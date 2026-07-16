using FileIntegrity.Services;
using FileIntegrityLab.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var authenticationServie = new AuthenticationService(configuration);

var graphclient = authenticationServie.GetGraphServiceClient();

var oneDriveService = new OneDriveService(graphclient);

await oneDriveService.DriveInfoAsync();

var user = await graphclient.Me.GetAsync();

Console.WriteLine($"Signed in as: {user?.DisplayName}");
Console.WriteLine($"Email: {user?.Mail}");