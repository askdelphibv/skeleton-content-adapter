using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    /// <summary>
    /// Very simple proof-of-concept resource repository implementation, backed by a local filesystem. Typically, all files offered are virtual.
    /// </summary>
    public class SampleDataResourceRepository : IResourceRepository
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<SampleDataResourceRepository> logger;
        private readonly DirectoryInfo dataFolder;
        private readonly string DefaultMimetype = "image/png";

        // To wait until the first load was completed in refresh, wait for this
        private ManualResetEvent dataLoaded = new ManualResetEvent(false);


        public SampleDataResourceRepository(IConfiguration configuration
            , ILogger<SampleDataResourceRepository> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.dataFolder = new DirectoryInfo(Path.Combine(configuration["DataFolder"], "resources"));
            if (!dataFolder.Exists)
            {
                throw new InvalidOperationException($"Can't find required folder '{dataFolder}'. Update appsettings.json to point DataFolder to an existing folder with a resources sub-folder.");
            }
        }

        public async Task<SCR<ResourceMetadata>> GetResourceMetadata(IOperationContext operationContext, string resourceId)
        {
            dataLoaded.WaitOne(); 

            FileInfo resourceFileInfo = new FileInfo(Path.Combine(dataFolder.FullName, resourceId));
            if (!resourceFileInfo.Exists)
            {
                return SCR<ResourceMetadata>.FromError(ErrorCodes.ResourceRepositoryFileNotFound, ErrorCodes.ResourceRepositoryFileNotFoundMessage, System.Net.HttpStatusCode.NotFound);
            }
            if (!Path.GetFullPath(resourceFileInfo.FullName).StartsWith(dataFolder.FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(); // error out
            }

            ResourceMetadata result = new()
            {
                Content = String.Empty,
                Description = resourceId,
                Tags = new TaxonomyValues[] { }
            };
            return SCR<ResourceMetadata>.FromData(await Task.FromResult(result));
        }

        public async Task<SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)>> GetResourcesList(IOperationContext operationContext, string folderId)
        {
            dataLoaded.WaitOne();

            DirectoryInfo parent = dataFolder;
            if (!string.IsNullOrEmpty(folderId))
            {
                parent = new DirectoryInfo(Path.Combine(dataFolder.FullName, folderId));
                if (!parent.Exists)
                {
                    return SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)>.FromError(ErrorCodes.ResourceRepositoryFileNotFound, ErrorCodes.ResourceRepositoryFileNotFoundMessage, System.Net.HttpStatusCode.NotFound);
                }
            }
            if (!Path.GetFullPath(parent.FullName).StartsWith(dataFolder.FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(); // error out
            }

            IEnumerable<FolderDescriptor> folders = parent.GetDirectories().Select(x => new FolderDescriptor
            {
                Name = x.Name,
                FolderId = Path.Combine(folderId ?? "", x.Name),
                LastModified = x.LastWriteTimeUtc.ToString("o")
            });
            IEnumerable<ResourceDescriptor> resources = parent.GetFiles().Select(x => FileInfoToResourceDescriptor(folderId, x));
            await Task.FromResult(0);
            return SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)>.FromData((folders: folders, resources: resources));
        }

        public async Task<SCR<(Stream resourceString, long contentLength, string contentType)>> GetResourceStream(IOperationContext operationContext, string resourceId)
        {
            dataLoaded.WaitOne();

            FileInfo resourceFileInfo = new FileInfo(Path.Combine(dataFolder.FullName, resourceId));
            if (!resourceFileInfo.Exists)
            {
                return SCR<(Stream resourceString, long contentLength, string contentType)>.FromError(ErrorCodes.ResourceRepositoryFileNotFound, ErrorCodes.ResourceRepositoryFileNotFoundMessage, System.Net.HttpStatusCode.NotFound);
            }
            if (!Path.GetFullPath(resourceFileInfo.FullName).StartsWith(dataFolder.FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(); // error out
            }
            await Task.FromResult(0);
            return SCR<(Stream resourceString, long contentLength, string contentType)>.FromData((resourceString: resourceFileInfo.OpenRead(), resourceFileInfo.Length, DefaultMimetype));

        }

        public async Task<SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)>> SearchForResource(IOperationContext operationContext, string query, int page, int size, string continuationToken)
        {
            dataLoaded.WaitOne();

            IEnumerable<ResourceDescriptor> found = await SearchForResourceInFolder(operationContext, query, dataFolder);
            var resultData = found.Skip(Math.Max(0, page - 1) * size).Take(size);
            return SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)>.FromData(
                (
                    resources: resultData,
                    totalCount: found.Count(),
                    continuationToken: $"{Guid.Empty}"
                )
                );
        }

        private  async Task<IEnumerable<ResourceDescriptor>> SearchForResourceInFolder(IOperationContext operationContext, string query, DirectoryInfo folder, string folderId = null)
        {
            if (!Path.GetFullPath(folder.FullName).StartsWith(dataFolder.FullName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(); // error out
            }
            List<ResourceDescriptor> result = new();
            foreach (var subFolder in folder.GetDirectories())
            {
                var children = await SearchForResourceInFolder(operationContext, query, subFolder, Path.Combine(folderId, subFolder.Name));
                result.AddRange(children);
            }
            var files = folder.GetFiles().Where(f => f.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) != -1).Select(x => FileInfoToResourceDescriptor(folderId, x));

            return result;
        }

        private ResourceDescriptor FileInfoToResourceDescriptor(string folderId, FileInfo x)
        {
            return new ResourceDescriptor
            {
                ContentLength = x.Length,
                Filename = x.Name,
                ResourceId = Path.Combine(folderId ?? string.Empty, x.Name),
                LastModified = x.LastWriteTimeUtc.ToString("o"),
                MimeType = DefaultMimetype,
                Status = "published"
            };
        }

        public async Task<SCR<bool>> RefreshAsync(IOperationContext operationContext)
        {
            // To check if the first load is completed, check the event was Set() already, if not we need to set it once
            // After it was set once, we will never set it again, we'll just load an additional collection in the 
            // background and swap out the one we have when we're done.
            bool firstLoadWasCompleted = dataLoaded.WaitOne(TimeSpan.FromSeconds(0));

            // If there was work to do to build a local cache, we would do it here

            if (!firstLoadWasCompleted)
            {
                dataLoaded.Set();
                logger.LogInformation($"First resource refresh completed.");
            }
            else
            {
                logger.LogInformation($"Resource refresh completed.");
            }
            return SCR<bool>.FromData(await Task.FromResult(true));
        }
    }
}
