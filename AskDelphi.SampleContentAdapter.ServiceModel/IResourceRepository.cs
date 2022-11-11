using AskDelphi.SampleContentAdapter.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.ServiceModel
{
    public interface IResourceRepository
    {
        Task<SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)>> GetResourcesList(IOperationContext operationContext, string folderId);
        Task<SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)>> SearchForResource(IOperationContext operationContext, string query, int page, int size, string continuationToken);
        Task<SCR<ResourceMetadata>> GetResourceMetadata(IOperationContext operationContext, string resourceId);
        Task<SCR<(Stream resourceString, long contentLength, string contentType)>> GetResourceStream(IOperationContext operationContext, string resourceId);
    }
}
