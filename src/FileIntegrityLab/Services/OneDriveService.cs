using Microsoft.Graph;

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
        }
    }
}
