using CloudMigrationTool.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationSource.OneDrive.OneDriveItems
{
    internal class OneDriveFile : ICloudFile
    {
        public string Extension => throw new NotImplementedException();

        public IDictionary<string, object> Attributes => throw new NotImplementedException();

        public DateTime CreationTime => throw new NotImplementedException();

        public DateTime CreationTimeUtc => throw new NotImplementedException();

        public ICloudDirectory Parent => throw new NotImplementedException();

        public string FullName => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public Stream Upload(Stream inputStream, bool overwriteIfExists = false, IProgress<long> progressCallback = null)
        {
            if (oneDriveCloudSource.IsAuthenticated && graphServiceClient != null)
            {
                if (Exists)
                {
                    // Use properties to specify the conflict behavior
                    // in this case, replace
                    var uploadSessionRequestBody = new CreateUploadSessionPostRequestBody
                    {
                        Item = new DriveItemUploadableProperties
                        {
                            AdditionalData = new Dictionary<string, object>
                            {
                                { "@microsoft.graph.conflictBehavior", "replace" }
                            }
                        }
                    };

                    var uploadSession = await graphServiceClient
                        .Drives[oneDriveCloudSource.UserDrive.Id]
                        .Items[Id]
                        .CreateUploadSession
                        .PostAsync(uploadSessionRequestBody);

                    var fileUploadTask = new LargeFileUploadTask<DriveItem>(
                        uploadSession: uploadSession,
                        uploadStream: inputStream,
                        requestAdapter: graphServiceClient.RequestAdapter);

                    var totalLength = inputStream.Length;

                    // Create a callback that is invoked after each slice is uploaded
                    IProgress<long> progress = new Progress<long>(prog => {
                        Console.WriteLine($"Uploaded {prog} bytes of {totalLength} bytes");

                        if( progressCallback != null)
                        {
                            progressCallback?.Report(prog);
                        }
                    });

                    try
                    {
                        // Upload the file
                        var uploadResult = await fileUploadTask.UploadAsync(progress);

                        Id = uploadResult.ItemResponse.Id;
                        Length = uploadResult.ItemResponse.Size;

                        Console.WriteLine(uploadResult.UploadSucceeded ?
                            $"Upload complete, item ID: {uploadResult.ItemResponse.Id}" :
                            "Upload failed");
                    }
                    catch (ServiceException ex)
                    {
                        Console.WriteLine($"Error uploading: {ex.ToString()}");
                    }
                }
            }
        }

        public bool Delete(bool permanent)
        {
            throw new NotImplementedException();
        }

        public Stream Open(FileMode mode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAttributes(IDictionary<string, object> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
