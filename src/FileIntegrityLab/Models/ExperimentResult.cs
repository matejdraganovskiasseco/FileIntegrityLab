public class ExperimentResult
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalHash { get; set; } = string.Empty;
    public string DownloadedHash { get; set; } = string.Empty;
    public bool IntegrityVerified { get; set; }
    public DateTime DateUploaded { get; set; }
    public DateTime DateDownloaded { get; set; }
}