using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    public class SampleDataResourceRepository : IResourceRepository
    {
        public Task<SCR<ResourceMetadata>> GetResourceMetadata(IOperationContext operationContext, string resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)>> GetResourcesList(IOperationContext operationContext, string folderId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<(Stream resourceString, long contentLength, string contentType)>> GetResourceStream(IOperationContext operationContext, string resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<bool>> RefreshAsync(IOperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public Task<SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)>> SearchForResource(IOperationContext operationContext, string query, int page, int size, string continuationToken)
        {
            throw new NotImplementedException();
        }
    }
}
