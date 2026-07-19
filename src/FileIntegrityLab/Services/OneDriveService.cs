using Microsoft.Graph;
using Microsoft.Graph.Models;
using DriveUpload = Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;

namespace FileIntegrity.Services
{
    public class OneDriveService
    {
        private readonly GraphServiceClient _graphClient;

        public OneDriveService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task DriveInfoAsync()
        {
            var drive = await _graphClient.Me.Drive.GetAsync();

            Console.WriteLine($"Drive name: {drive?.Name}");
            Console.WriteLine($"Drive type: {drive?.DriveType}");
            Console.WriteLine($"Drive Id : {drive?.Id}");
        }

        public async Task<DriveItem> GetOrCreateFolderAsync(string folderName)
        {
            var drive = await _graphClient.Me.Drive.GetAsync();

            var children = await _graphClient
                .Drives[drive!.Id!]
                .Items["root"]
                .Children
                .GetAsync();

            var existingFolder = children?.Value?
                .FirstOrDefault(item =>
                    item.Folder != null &&
                    item.Name == folderName);

            if (existingFolder != null)
            {
                Console.WriteLine($"Using existing folder: {folderName}");
                return existingFolder;
            }

            var folder = new DriveItem
            {
                Name = folderName,
                Folder = new Folder()
            };

            var createdFolder = await _graphClient
                .Drives[drive.Id!]
                .Items["root"]
                .Children
                .PostAsync(folder);

            Console.WriteLine($"Created folder: {folderName}");

            return createdFolder!;
        }

        public async Task<DriveItem?> UploadFileAsync(string filePath, string folderId)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var drive = await _graphClient.Me.Drive.GetAsync();

            using var fileStream = File.OpenRead(filePath);

            var uploadBody = new DriveUpload.CreateUploadSessionPostRequestBody
            {
                Item = new DriveItemUploadableProperties
                {
                    AdditionalData = new Dictionary<string, object>
            {
                { "@microsoft.graph.conflictBehavior", "replace" }
            }
                }
            };

            var uploadSession = await _graphClient
                .Drives[drive!.Id!]
                .Items[$"{folderId}:/{Path.GetFileName(filePath)}:"]
                .CreateUploadSession
                .PostAsync(uploadBody);

            int maxSliceSize = 320 * 1024;

            var uploadTask = new LargeFileUploadTask<DriveItem>(
                uploadSession!,
                fileStream,
                maxSliceSize,
                _graphClient.RequestAdapter);

            var result = await uploadTask.UploadAsync();

            if (result.UploadSucceeded)
            {
                Console.WriteLine("Upload completed successfully.");
                return result.ItemResponse;
            }

            return null;
        }

        public async Task DownloadFileAsync(string fileId, string destionationPath)
        {
            var drive = await _graphClient.Me.Drive.GetAsync();

            var stream = await _graphClient
                .Drives[drive!.Id!]
                .Items[fileId]
                .Content
                .GetAsync();

            if(stream == null)
            {
                throw new InvalidOperationException("unable to download file");
            }

            using var filestream = File.Create(destionationPath);

            await stream.CopyToAsync(filestream);

            Console.WriteLine("file downloaded successfully");
        }
    }
}
