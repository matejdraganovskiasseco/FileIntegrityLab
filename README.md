# FileIntegrityLab

A .NET 8 console application that demonstrates secure file integrity verification using Microsoft Graph and OneDrive.

The application authenticates a user with Microsoft Entra ID, uploads a file to OneDrive, downloads the same file, computes SHA-256 hashes for both copies, and verifies that the file remained unchanged during transfer.

---

## Features

- Microsoft Entra ID authentication using Device Code Flow
- Microsoft Graph SDK integration
- Retrieve OneDrive drive information
- Create or reuse a destination folder
- Upload files to OneDrive
- Download files from OneDrive
- SHA-256 integrity verification
- Console report summarizing the experiment
- Configuration through `appsettings.json`

---

## Technologies

- .NET 8
- C#
- Microsoft Graph SDK v6
- Azure Identity
- Microsoft.Extensions.Configuration

---

## Project Structure

```
FileIntegrityLab
│
├── DownloadedFiles/
├── TestFiles/
│   └── sample.txt
│
├── Services/
│   ├── AuthenticationService.cs
│   ├── HashService.cs
│   ├── OneDriveService.cs
│   └── ReportService.cs
│
├── Models/
│   └── ExperimentResult.cs
│
├── Program.cs
├── appsettings.json
└── README.md
```

---
## How It Works

1. Authenticate using Microsoft Entra ID (Device Code Flow)
2. Connect to Microsoft Graph
3. Retrieve OneDrive information
4. Create or reuse the configured OneDrive folder
5. Upload the sample file
6. Download the uploaded file
7. Compute SHA-256 hashes for both files
8. Compare the hashes
9. Print an integrity verification report

---

## Configuration

Configure your Azure application in `appsettings.json`.

```json
{
  "AzureAd": {
    "ClientId": "YOUR_CLIENT_ID"
  },
  "OneDrive": {
    "FolderName": "FileIntegrityLab"
  },
  "Paths": {
    "TestFilesFolder": "TestFiles",
    "DownloadedFilesFolder": "DownloadedFiles",
    "SampleFile": "sample.txt"
  }
}
```

---

## Required Microsoft Graph Permissions

The application uses delegated permissions:

- User.Read
- Files.ReadWrite

Authentication is performed using the Device Code Flow.

---

## Example Output

```
Signed in as: John Doe
Email: john.doe@contoso.com

Using existing folder: FileIntegrityLab

Uploaded: sample.txt
File Id : 0123456789ABCDEF

Downloaded successfully.

========================================
        FILE INTEGRITY REPORT
========================================

File Name          : sample.txt

Original SHA-256   :
9D8A...

Downloaded SHA-256 :
9D8A...

Integrity Verified : YES 

========================================
```

---

## Integrity Verification

The application calculates a SHA-256 hash before uploading the file and again after downloading it.

If both hashes are identical, the integrity of the transferred file is verified.

---

## Design

The application follows a simple service-oriented architecture.

### Services

- **AuthenticationService** – Creates the Microsoft Graph client.
- **OneDriveService** – Handles OneDrive operations.
- **HashService** – Computes SHA-256 hashes.
- **ReportService** – Displays the experiment results.

### Model

- **ExperimentResult** – Stores the outcome of the integrity verification.

---

## Future Improvements

Potential enhancements include:

- Support for multiple files
- Recursive folder uploads
- Progress reporting
- Logging with Microsoft.Extensions.Logging
- Unit tests
- JSON or PDF report generation
- Additional hash algorithms (SHA-512, MD5)
- Getting the configuration fron Environment Variables.

