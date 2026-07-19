using FileIntegrity.Services;
using FileIntegrityLab.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var oneDriveFolder = configuration["OneDrive:FolderName"]!;
var testFolder = configuration["Paths:TestFilesFolder"]!;
var downloadFolder = configuration["Paths:DownloadedFilesFolder"]!;
var sampleFile = configuration["Paths:SampleFile"]!;

var authenticationServie = new AuthenticationService(configuration);

var graphclient = authenticationServie.GetGraphServiceClient();

var oneDriveService = new OneDriveService(graphclient);
await oneDriveService.DriveInfoAsync();

var user = await graphclient.Me.GetAsync();

Console.WriteLine($"Signed in as: {user?.DisplayName}");
Console.WriteLine($"Email: {user?.Mail}");

var folder = await oneDriveService.GetOrCreateFolderAsync(oneDriveFolder!);

var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!
    .Parent!
    .Parent!
    .Parent!
    .FullName;

var testFile = Path.Combine(
    projectRoot,
    testFolder!,
    sampleFile!);

var uploadedFile = await oneDriveService.UploadFileAsync(testFile, folder.Id!);

Console.WriteLine($"Uploaded: {uploadedFile?.Name}");
Console.WriteLine($"File Id : {uploadedFile?.Id}");

var downloadedFolder = Path.Combine(
    projectRoot,
    downloadFolder!);

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
    IntegrityVerified = integrity
};

var reportService = new ReportService();
reportService.PrintReport(result);