# Experiment Report

## Hypothesis

Uploading a file to Microsoft OneDrive through the Microsoft Graph API and downloading it again will preserve the file's integrity.

---

## Was the hypothesis confirmed?

Yes.

The SHA-256 hash calculated before the upload matched the SHA-256 hash calculated after downloading the file. This indicates that the file contents remained unchanged during transfer.

---

## Challenges Encountered

Several challenges were encountered during development:

- Configuring Microsoft Entra ID and Microsoft Graph permissions correctly.
- Understanding the Microsoft Graph SDK request structure.
- Handling cases where the destination folder already existed.
- Managing file paths through application configuration.

Each issue was resolved through incremental testing and improvements to the application.

---

## Future Experiments

Possible future improvements include:

- Testing with large files (100 MB+).
- Comparing upload and download performance for different file sizes and formats.
- Verifying integrity for multiple files in a batch.
- Supporting additional hash algorithms such as SHA-512.
- Automating repeated integrity verification.
- Comparing integrity verification across different cloud storage providers.

---

## Final Remarks

The experiment successfully demonstrated how Microsoft Graph can be used to automate secure file transfers while verifying data integrity through cryptographic hashing. The modular service-based architecture also makes the application easy to extend with additional functionality.
