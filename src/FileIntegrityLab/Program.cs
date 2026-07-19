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

var folder = await oneDriveService.GetOrCreateFolderAsync(configuration["OneDrive:FolderName"]!);

var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!
    .Parent!
    .Parent!
    .Parent!
    .FullName;

var testFile = Path.Combine(
    projectRoot,
    configuration["Paths:TestFilesFolder"]!,
    configuration["Paths:SampleFile"]!);

var uploadedFile = await oneDriveService.UploadFileAsync(testFile, folder.Id!);

Console.WriteLine($"Uploaded: {uploadedFile?.Name}");
Console.WriteLine($"File Id : {uploadedFile?.Id}");

var downloadedFolder = Path.Combine(
    projectRoot,
    configuration["Paths:DownloadedFilesFolder"]!);

Directory.CreateDirectory(downloadedFolder);

var downloadedFile = Path.Combine(
    downloadedFolder,
    uploadedFile!.Name!);

await oneDriveService.DownloadFileAsync(
    uploadedFile.Id!,
    downloadedFile);

var hashService = new HashService();

var originalHash = hashService.ComputeHash(testFile);
var downloadedHash = hashService.ComputeHash(downloadedFile);

bool integrity = originalHash == downloadedHash;

var result = new ExperimentResult
{
    FileName = uploadedFile.Name!,
    OriginalHash = originalHash,
    DownloadedHash = downloadedHash,
    IntegrityVerified = integrity,
    DateUploaded = DateTime.UtcNow,
    DateDownloaded = DateTime.UtcNow
};

Console.WriteLine($"Original Hash: {result.OriginalHash} From the file: {result.FileName}");
Console.WriteLine($"Downloaded Hash: {result.DownloadedHash} From the file: {result.FileName}");

Console.WriteLine($"Integrity Verified: {(result.IntegrityVerified ? "yes" : "no")}");