using FileIntegrity.Services;
using FileIntegrityLab.Services;
using Microsoft.Extensions.Configuration;

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var oneDriveFolder = configuration["OneDrive:FolderName"]!;
var testFolder = configuration["Paths:TestFilesFolder"]!;
var downloadFolder = configuration["Paths:DownloadedFilesFolder"]!;
var sampleFile = configuration["Paths:SampleFile"]!;

// Authentication
var authenticationService = new AuthenticationService(configuration);
var graphClient = authenticationService.GetGraphServiceClient();

var user = await graphClient.Me.GetAsync();

Console.WriteLine($"Signed in as: {user?.DisplayName}");
Console.WriteLine($"Email: {user?.Mail}");

// OneDrive
var oneDriveService = new OneDriveService(graphClient);

await oneDriveService.DriveInfoAsync();

var folder = await oneDriveService.GetOrCreateFolderAsync(oneDriveFolder);

// Build file paths
var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!
    .Parent!
    .Parent!
    .Parent!
    .FullName;

var testFilePath = Path.Combine(projectRoot, testFolder, sampleFile);

var downloadedFolderPath = Path.Combine(projectRoot, downloadFolder);
Directory.CreateDirectory(downloadedFolderPath);

// Upload
var uploadedFile = await oneDriveService.UploadFileAsync(testFilePath, folder.Id!);

Console.WriteLine($"Uploaded: {uploadedFile?.Name}");
Console.WriteLine($"File Id : {uploadedFile?.Id}");

// Download
var downloadedFilePath = Path.Combine(downloadedFolderPath, uploadedFile!.Name!);

await oneDriveService.DownloadFileAsync(
    uploadedFile.Id!,
    downloadedFilePath);

// Verify integrity
var hashService = new HashService();

var originalHash = hashService.ComputeHash(testFilePath);
var downloadedHash = hashService.ComputeHash(downloadedFilePath);

var result = new ExperimentResult
{
    FileName = uploadedFile.Name!,
    OriginalHash = originalHash,
    DownloadedHash = downloadedHash,
    IntegrityVerified = originalHash == downloadedHash
};

// Report
var reportService = new ReportService();
reportService.PrintReport(result);