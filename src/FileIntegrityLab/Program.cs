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

var folder = await oneDriveService.CreateFolderAsync();

var testFile = Path.Combine(
    Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName,
    "TestFiles",
    "sample.txt");

var uploadedFile = await oneDriveService.UploadFileAsync(testFile, folder.Id!);

Console.WriteLine($"Uploaded: {uploadedFile?.Name}");
Console.WriteLine($"File Id : {uploadedFile?.Id}");

var downloadedFile = Path.Combine(
    @"C:\projects\FileIntegrityLab\DownloadedFiles",
    uploadedFile!.Name!);

await oneDriveService.DownloadFileAsync(
    uploadedFile.Id!,
    downloadedFile);

var hashService = new HashService();

var originalHash = hashService.ComputeHash(testFile);
var downloadedHash = hashService.ComputeHash(downloadedFile);

Console.WriteLine($"Original Hash: {originalHash} From the file: {testFile}");
Console.WriteLine($"Downloaded Hash: {downloadedHash} From the file: {downloadedFile}");

bool integrity = originalHash == downloadedHash;

Console.WriteLine($"Integrity Verified: {(integrity ? "yes" : "no")}");

