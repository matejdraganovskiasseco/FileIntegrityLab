using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace FileIntegrityLab.Services;

public class AuthenticationService
{
    private readonly IConfiguration _configuration;
    
    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public GraphServiceClient GetGraphServiceClient()
    {
        var clientId = _configuration["AzureAd:ClientId"];

        if (string.IsNullOrEmpty(clientId)) {
            throw new InvalidOperationException("Azure Ad client id is missing in appsetings.json");
        }

        var credential = new DeviceCodeCredential(
            new DeviceCodeCredentialOptions
            {
                ClientId = clientId,
                TenantId = "common",

                DeviceCodeCallback = (callback, cancellationToken) =>
                {
                    Console.WriteLine(callback.Message);
                    return Task.CompletedTask;
                }
            });

        var scopes = new[] { "User.Read", "Files.ReadWrite" };
        
        return new GraphServiceClient(credential, scopes);
    }
}