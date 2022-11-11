using AskDelphi.SampleContentAdapter.DTO;
using AskDelphi.SampleContentAdapter.ServiceModel;
using AskDelphi.SampleContentAdapter.ServiceModel.DTO;
using AskDelphi.SampleContentAdapter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Controllers
{
    /// <summary>
    /// For interacting with resources.
    /// </summary>
    [Route("api/resources")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly ILogger<ContentController> logger;
        private readonly IResourceRepository resourceRepository;


        /// <summary>
        /// 
        /// </summary>    
        public ResourcesController(ILogger<ContentController> logger, IResourceRepository resourceRepository)
        {
            this.logger = logger;
            this.resourceRepository = resourceRepository;
        }

        /// <summary>
        /// List all resources in a specific folder.
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<ResourcesListResponse> GetResourcesList([FromQuery] string folderId = null)
        {
            ResourcesListResponse response = new ResourcesListResponse(HttpContext.GetOperationContext());
            SCR<(IEnumerable<FolderDescriptor> folders, IEnumerable<ResourceDescriptor> resources)> getResourcesListSCR = await resourceRepository.GetResourcesList(HttpContext.GetOperationContext(), folderId);
            response.InitializeFromSCR(getResourcesListSCR);
            return response;
        }

        /// <summary>
        /// Search for resources using certain parameters.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="continuationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<ResourcesSearchResponse> GetResourcesSearch([FromQuery] string query = null, [FromQuery] int page = 1, [FromQuery] int size = 100, [FromQuery] string continuationToken = null)
        {
            ResourcesSearchResponse response = new ResourcesSearchResponse(HttpContext.GetOperationContext());
            SCR<(IEnumerable<ResourceDescriptor> resources, int totalCount, string continuationToken)> searchResourceSCR = await resourceRepository.SearchForResource(HttpContext.GetOperationContext(), query, page, size, continuationToken);
            response.InitializeFromSCR(searchResourceSCR);
            return response;
        }

        /// <summary>
        /// Return resource metadata for a specific resource.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("metadata")]
        public async Task<ResourcesMetadataResponse> GetResourcesMetadata([FromQuery] string resourceId)
        {
            ResourcesMetadataResponse response = new ResourcesMetadataResponse(HttpContext.GetOperationContext());
            SCR<ResourceMetadata> scr = await resourceRepository.GetResourceMetadata(HttpContext.GetOperationContext(), resourceId);
            response.InitializeFromSCR(scr);
            return response;
        }

        /// <summary>
        /// HEAD method for partial downloads.
        /// </summary>
        /// <param name="resourceId">Target resource id</param>
        /// <returns></returns>
        [HttpHead]
        [Route("content")]
        public async Task HeadResourcesContent([FromQuery] string resourceId)
        {
            SCR<(Stream resourceStream, long contentLength, string contentType)> scr = await resourceRepository.GetResourceStream(HttpContext.GetOperationContext(), resourceId);
            if (scr.IsError)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                Response.ContentLength = scr.Result.contentLength;
                Response.ContentType = scr.Result.contentType;
                Response.Headers.Add("Accept-Ranges", "bytes");
                Response.StatusCode = StatusCodes.Status200OK;
            }
        }

        /// <summary>
        /// Gets the resource binary content, supports partial downloads.
        /// </summary>
        /// <param name="resourceId">Target resource id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("content")]
        public async Task<IActionResult> GetResourcesContent([FromQuery] string resourceId)
        {
            SCR<(Stream resourceStream, long contentLength, string contentType)> scr = await resourceRepository.GetResourceStream(HttpContext.GetOperationContext(), resourceId);
            if (scr.IsError)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                throw new FileNotFoundException();
            }
            else
            {
                return File(scr.Result.resourceStream, scr.Result.contentType, enableRangeProcessing: true);
            }
        }
    }
}
