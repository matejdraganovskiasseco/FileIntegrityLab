namespace FileIntegrity.Services
{
    public class ReportService
    {
        public void PrintReport(ExperimentResult result)
        {
            Console.WriteLine();

            Console.WriteLine("========================================");
            Console.WriteLine("        FILE INTEGRITY REPORT");
            Console.WriteLine("========================================");

            Console.WriteLine($"File Name          : {result.FileName}");

            Console.WriteLine();

            Console.WriteLine($"Original SHA-256   : {result.OriginalHash}");
            Console.WriteLine($"Downloaded SHA-256 : {result.DownloadedHash}");

            Console.WriteLine();

            Console.WriteLine($"Integrity Verified : {(result.IntegrityVerified ? "Yes" : "No")}");

            Console.WriteLine("========================================");
        }
    }
}
