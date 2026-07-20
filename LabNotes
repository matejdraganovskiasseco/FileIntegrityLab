# Lab Notes

## Objective

The objective of this experiment was to verify whether a file maintains its integrity after being uploaded to and downloaded from Microsoft OneDrive using the Microsoft Graph API by comparing the computed SHA-256 values.

---

## Environment

- .NET 8
- C#
- Microsoft Graph SDK v6
- Azure Identity
- Microsoft Entra ID
- Windows 11

---

## Setup

- Registered an Azure application.
- Configured Microsoft Graph delegated permissions:
  - User.Read
  - Files.ReadWrite
- Configured the application using `appsettings.json`.
- Used Device Code Flow for authentication.
- Created a sample text file for testing with some text value inside it.

---

## Experiment Steps

1. Authenticate the user.
2. Connect to Microsoft Graph.
3. Retrieve OneDrive information.
4. Create or reuse a OneDrive folder.
5. Upload the sample file.
6. Download the uploaded file.
7. Compute SHA-256 hashes of both files.
8. Compare the hashes.
9. Display the results.

---

## Results

The uploaded file was successfully stored in OneDrive and downloaded without errors.

The SHA-256 hash of the original file matched the SHA-256 hash of the downloaded file.

Result:

- Upload: Successful
- Download: Successful
- Hash Match: Yes
- Integrity Verified: Yes

---

## Errors Encountered

During development,i encountered several issues:

- Microsoft Graph returned a "Name already exists" error when attempting to recreate an existing folder. This was resolved by implementing a "Get or Create Folder" method.
- Incorrect configuration key names caused path resolution issues. Updating the configuration resolved the problem.
- Initial testing required validating the Microsoft Graph permissions and authentication flow.
- Microsoft Graph Returned "507 Insufficient Storage" because my Drive storage was full.

---

## Conclusion

The experiment demonstrated that Microsoft Graph successfully preserved file integrity during upload and download operations. SHA-256 verification confirmed that the downloaded file was identical to the original.
