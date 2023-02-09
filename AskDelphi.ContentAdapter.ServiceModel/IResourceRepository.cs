using AskDelphi.ContentAdapter.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.ServiceModel
{
    /// <summary>
    /// Interface to the resource repository for this adapter, this contains all resources that are managed by the adapter.
    /// </summary>
    public interface IResourceRepository
    {
        /// <summary>
        /// Returns the list of resources and subfolders in a specific resource folder)
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        Task<SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)>> GetResourcesList(IOperationContext operationContext, string folderId);

        /// <summary>
        /// Re
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        Task<SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)>> SearchForResource(IOperationContext operationContext, string query, int page, int size, string continuationToken);

        /// <summary>
        /// Returns descriptive metadata for a hosted resource.
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<SCR<ResourceMetadata>> GetResourceMetadata(IOperationContext operationContext, string resourceId);

        /// <summary>
        /// Returns the resource as a stream
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<SCR<(Stream resourceString, long contentLength, string contentType)>> GetResourceStream(IOperationContext operationContext, string resourceId);

        /// <summary>
        /// Requests a refresh.
        /// </summary>
        /// <returns>True if refresh was started, false otherwise.</returns>
        Task<SCR<bool>> RefreshAsync(IOperationContext operationContext);
    }
}
