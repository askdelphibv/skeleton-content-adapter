using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Controllers
{
    /// <summary>
    /// For interacting with content.
    /// </summary>
    [Route("api/content")]
    [ApiController]
    [Authorize]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> logger;
        private readonly ITopicRepository topicRepository;

        /// <summary></summary>
        public ContentController(ILogger<ContentController> logger, ITopicRepository topicRepository)
        {
            this.logger = logger;
            this.topicRepository = topicRepository;
        }

        /// <summary>
        /// Returns all sub-folders for a folder.
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("folders")]
        public async Task<ContentFoldersResponse> GetFolders([FromQuery] string folderId = null)
        {
            ContentFoldersResponse response = new ContentFoldersResponse(HttpContext.GetOperationContext());

            SCR<IEnumerable<FolderDescriptor>> scr = await topicRepository.FindFoldersAsync(HttpContext.GetOperationContext(), folderId);
            Response.StatusCode = response.Initialize(scr);
            
            return response;
        }

        /// <summary>
        /// Used to search for content using specific criteria.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ContentSearchResponse> PostSearch([FromBody] ContentSearchRequest request)
        {
            ContentSearchResponse response = new ContentSearchResponse(HttpContext.GetOperationContext());

            SCR<IEnumerable<TopicDescriptor>> scr = await topicRepository.FindMatchingTopicsAsync(HttpContext.GetOperationContext(), request);
            Response.StatusCode = response.Initialize(scr);

            return response;

        }

        /// <summary>
        /// Requests metadata of a content item.
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("metadata")]
        public async Task<ContentMetadataResponse> GetMetadata([FromQuery] string topicId)
        {
            ContentMetadataResponse response = new ContentMetadataResponse(HttpContext.GetOperationContext());
            
            SCR<TopicMetadata> scr = await topicRepository.GetMetadata(HttpContext.GetOperationContext(), topicId);
            Response.StatusCode = response.Initialize(scr);

            return response;
        }

        /// <summary>
        /// Request all topics and related content topics of a specific content item.
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("content")]
        public async Task<ContentContentResponse> GetContent([FromQuery] string topicId)
        {
            ContentContentResponse response = new ContentContentResponse(HttpContext.GetOperationContext());            
            SCR<TopicContent[]> scr = await topicRepository.GetContent(HttpContext.GetOperationContext(), topicId);
            Response.StatusCode = response.Initialize(scr);

            return response;
        }

        /// <summary>
        /// Requests the date and time the content identified by the topic id was last changed. If the last changed date is unknown,
        /// or the content has not been modified, the system may return a null value.
        /// </summary>
        /// <param name="topicId">Request topic id</param>
        /// <returns>A datestamp with timezone indication indicating when the content was last changed.</returns>
        [HttpGet]
        [Route("lastchangeddate")]
        public async Task<ContentLastChangedDateResponse> GetContentLastChangedDate([FromQuery] string topicId)
        {
            ContentLastChangedDateResponse response = new ContentLastChangedDateResponse(HttpContext.GetOperationContext());
            SCR<DateTimeOffset?> scr = await topicRepository.GetContentLastChangedDate(HttpContext.GetOperationContext(), topicId);
            Response.StatusCode = response.Initialize(scr);

            return response;
        }
    }
}
