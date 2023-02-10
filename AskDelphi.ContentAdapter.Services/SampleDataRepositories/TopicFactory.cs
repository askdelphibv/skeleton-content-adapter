using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services.Utilities;
using Imola.API.Interop.Content;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TopicFactory : ITopicFactory
    {
        private readonly ICMSAdapter cmsAdapter;
        private readonly FileExtensionContentTypeProvider fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        public TopicFactory(ICMSAdapter cmsAdapter)
        {
            this.cmsAdapter = cmsAdapter;
        }

        public async Task<SCR<TopicContent[]>> CreateTopic(IOperationContext operationContext, string topicId, FileInfo file)
        {
            List<TopicContent> result = new();
            ProcessEntry process = LoadContents(file);

            // The file always represents one single process, followed by a collection of tasks, and an image topic representing the thumbnail, referring to the thumbnail resource by ID

            string processTopicId = GetProcessTopicID(topicId);

            // Add the content topics
            TopicContent imageTopic = CreateImageTopic(operationContext, process, $"{processTopicId}/{process.Thumbnail}");

            result.Add(await CreateProcessTopic(operationContext, process, processTopicId, imageTopic));
            result.Add(imageTopic);
            if (null != process.Tasks)
            {
                result.AddRange(process.Tasks.Select(x => CreateTaskTopic(operationContext, x, $"{processTopicId}/{x.Id}")));
            }

            SCR<TopicContent[]> response = await Task.FromResult(SCR<TopicContent[]>.FromData(result.ToArray()));
            if (IsChildTopic(topicId))
            {
                // If this is not the 'root topic'
                MoveRequestedChildToHeadOfTheList(topicId, response);
            }
            return response;
        }

        private TopicContent CreateImageTopic(IOperationContext operationContext, ProcessEntry process, string topicId)
        {
            string resourcePath = $"remote-resource://{operationContext.GetAskDelphiSystemID()}/{process.Thumbnail}";
            if (!fileExtensionContentTypeProvider.TryGetContentType(System.IO.Path.GetFileName(process.Thumbnail), out string mimeType))
            {
                mimeType = "application/octet-stream";
            }
            return new TopicContent
            {
                BasicData = new TopicContentBasicData
                {
                    Description = string.Empty,
                    Enabled = true,
                    IsDescriptionCalculated = false,
                    IsEmpty = false,
                    IsPublished = true,
                    MetricsTags = new string[] { },
                    ModificationDate = DateTime.UtcNow.ToString("o"),
                    Namespace = AskDelphiCMSAdapter.ImageTopicNamespace,
                    TopicTitle = $"Thumbnail for {process.Title}",
                    TopicTitleMarkup = $"Thumbnail for {process.Title}",
                    TopicType = AskDelphiCMSAdapter.ImageTopicTypeTitle,
                    Version = AskDelphiCMSAdapter.DefaultVersion
                },
                Content = CreateDoppioImageTopicJSON(process, resourcePath, mimeType),
                Guid = GuidUtility.Create(GuidUtility.UrlNamespace, resourcePath), // the path is unique for this resource so perfect for creating a guid from
                Relations = new TopicContentRelationData
                {
                    References = new List<TopicContentRelationReference>(),
                },
                TopicId = topicId
            };
        }

        private static string CreateDoppioImageTopicJSON(ProcessEntry process, string resourcePath, string mimeType)
        {
            return JsonSerializer.Serialize(new Imola.API.Interop.Content.ImageTopic
            {
                Title = $"Thumbnail for {process.Title}",
                Description = process.Content,
                Guid = GuidUtility.Create(GuidUtility.UrlNamespace, resourcePath),
                IsPublished = true,
                OriginalTitle = $"Thumbnail for {process.Title}",
                TitleMarkup = $"Thumbnail for {process.Title}",
                TopicLearning = null,
                TopicType = AskDelphiCMSAdapter.ImageTopicTypeTitle,
                LocalPathExtraLarge = resourcePath,
                LocalPathLarge = resourcePath,
                LocalPathMedium = resourcePath,
                LocalPathRaw = resourcePath,
                LocalPathSmall = resourcePath,
                MimeType = mimeType
            });
        }

        private async Task<TopicContent> CreateProcessTopic(IOperationContext operationContext, ProcessEntry process, string topicId, TopicContent thumbnailTolpic)
        {
            int i = 0;
            Guid processTaskRelationTypeKey = await cmsAdapter.GetRelationTypeKeyFor(operationContext, AskDelphiCMSAdapter.ProcessTaskRelationPyramidLevelTitle, 
                AskDelphiCMSAdapter.TaskTopicTypeTitle, AskDelphiCMSAdapter.TaskTopicNamespace, "Relation", "Default");
            TopicContent result = new TopicContent
            {
                BasicData = new TopicContentBasicData
                {
                    Description = string.Empty,
                    Enabled = true,
                    IsDescriptionCalculated = false,
                    IsEmpty = false,
                    IsPublished = true,
                    MetricsTags = new string[] { },
                    ModificationDate = DateTime.UtcNow.ToString("o"),
                    Namespace = AskDelphiCMSAdapter.ProcessTopicNamespace,
                    TopicTitle = process.Title,
                    TopicTitleMarkup = process.Title,
                    TopicType = AskDelphiCMSAdapter.ProcessTopicTypeTitle,
                    Version = AskDelphiCMSAdapter.DefaultVersion
                },
                Content = CreateImolaProcessTopicJSON(process),
                Guid = new Guid(process.Id),
                Relations = new TopicContentRelationData
                {
                    ThumbnailTopicGuid = $"{thumbnailTolpic.Guid}",
                    References = process.Tasks.Select(x => new TopicContentRelationReference
                    {
                        IsInferredFromContent = false,
                        Metadata = new List<DTO.KeyValuePair>(),
                        PyramidLevel = AskDelphiCMSAdapter.ProcessTaskRelationPyramidLevelTitle,
                        RelationTypeKey = processTaskRelationTypeKey,
                        SequenceNumber = ++i,
                        TargetTopicGuid = new Guid(x.Id),
                        TargetTopicNamespaceUri = AskDelphiCMSAdapter.TaskTopicNamespace,
                        TargetTopicTitle = x.Title,
                        Use = "Task",
                        View = "Default"
                    }).ToList(),
                },
                TopicId = topicId
            };

            result.Relations.References.Add(new TopicContentRelationReference
            {
                IsInferredFromContent = true,
                Metadata = new List<DTO.KeyValuePair>(),
                PyramidLevel = null,
                RelationTypeKey = Guid.Empty,
                SequenceNumber = ++i,
                TargetTopicGuid = thumbnailTolpic.Guid,
                TargetTopicNamespaceUri = AskDelphiCMSAdapter.ImageTopicNamespace,
                TargetTopicTitle = thumbnailTolpic.BasicData.TopicTitle,
                Use = "Thumbnail",
                View = "Default"
            });
            return result;
        }

        private static string CreateImolaProcessTopicJSON(ProcessEntry process)
        {
            return JsonSerializer.Serialize(new Imola.API.Interop.Content.ProcessTopic
            {
                Title = process.Title,
                BodyHTML = process.Content,
                Description = process.Content,
                Guid = new Guid(process.Id),
                IsBodyEmpty = false,
                IsPublished = true,
                Keywords = string.Empty,
                OriginalTitle = process.Title,
                TitleMarkup = process.Title,
                TopicLearning = null,
                TopicType = AskDelphiCMSAdapter.ProcessTopicTypeTitle
            });
        }

        private TopicContent CreateTaskTopic(IOperationContext operationContext, ProcessEntry task, string taskTopicId)
        {
            return new TopicContent
            {
                BasicData = new TopicContentBasicData
                {
                    Description = string.Empty,
                    Enabled = true,
                    IsDescriptionCalculated = false,
                    IsEmpty = false,
                    IsPublished = true,
                    MetricsTags = new string[] { },
                    ModificationDate = DateTime.UtcNow.ToString("o"),
                    Namespace = AskDelphiCMSAdapter.TaskTopicNamespace,
                    TopicTitle = task.Title,
                    TopicTitleMarkup = task.Title,
                    TopicType = AskDelphiCMSAdapter.TaskTopicTypeTitle,
                    Version = AskDelphiCMSAdapter.DefaultVersion
                },
                Content = CreateImolaTaskTopicJSON(task),
                Guid = new Guid(task.Id),
                Relations = new TopicContentRelationData(),
                TopicId = taskTopicId
            };
        }

        private static string CreateImolaTaskTopicJSON(ProcessEntry task)
        {
            return JsonSerializer.Serialize(new Imola.API.Interop.Content.TaskTopic
            {
                Title = task.Title,
                BodyHTML = task.Content,
                Description = task.Content,
                Guid = new Guid(task.Id),
                IsBodyEmpty = false,
                IsPublished = true,
                Keywords = string.Empty,
                OriginalTitle = task.Title,
                TitleMarkup = task.Title,
                TopicLearning = null,
                TopicType = AskDelphiCMSAdapter.TaskTopicTypeTitle
            });
        }

        private ProcessEntry LoadContents(FileInfo file)
        {
            using (StreamReader stream = file.OpenText())
            {
                string data = stream.ReadToEnd();
                var foo = Newtonsoft.Json.JsonConvert.DeserializeObject<ProcessEntry>(data);
                return foo;
            }
        }

        private TopicDescriptor ConvertToDescriptor(ProcessEntry contents, string id, string ttTitle, string ttNamespace)
        {
            return new TopicDescriptor
            {
                TopicId = id,
                Guid = new Guid(contents.Id),
                Status = string.Empty,
                Title = contents.Title,
                Namespace = ttNamespace,
                Type = ttTitle,
                Version = AskDelphiCMSAdapter.DefaultVersion
            };
        }

        private static string GetProcessTopicID(string topicId)
        {
            return '/' + topicId.Trim('/').Split('/').FirstOrDefault();
        }

        private bool IsChildTopic(string topicId)
        {
            return topicId.Trim('/').Contains('/');
        }

        public async Task<IEnumerable<TopicDescriptor>> GetDescriptors(IOperationContext operationContext, FileInfo file, string topicId)
        {
            ProcessEntry contents = LoadContents(file);

            List<TopicDescriptor> result = new()
            {
                ConvertToDescriptor(contents, topicId, AskDelphiCMSAdapter.ProcessTopicTypeTitle, AskDelphiCMSAdapter.ProcessTopicNamespace)
            };
            if (null != contents.Tasks) result.AddRange(contents.Tasks.Select(x => ConvertToDescriptor(x, $"{topicId}/{x.Id}", AskDelphiCMSAdapter.TaskTopicTypeTitle, AskDelphiCMSAdapter.TaskTopicNamespace)));
            TopicContent imageTopic = CreateImageTopic(operationContext, contents, $"{topicId}/{contents.Thumbnail}");
            result.Add(TopicContentToDescriptor(imageTopic));

            return await Task.FromResult(result);
        }

        private static TopicDescriptor TopicContentToDescriptor(TopicContent imageTopic)
        {
            return new TopicDescriptor
            {
                TopicId = imageTopic.TopicId,
                Guid = imageTopic.Guid,
                Status = string.Empty,
                Title = imageTopic.BasicData.TopicTitle,
                Namespace = imageTopic.BasicData.Namespace,
                Type = imageTopic.BasicData.TopicType,
                Version = imageTopic.BasicData.Version
            };
        }

        private void MoveRequestedChildToHeadOfTheList(string topicId, SCR<TopicContent[]> response)
        {
            TopicContent firstItem = response.Result.Where(x => x.TopicId == topicId).FirstOrDefault();
            List<TopicContent> list = response.Result.ToList();
            list.RemoveAll(x => x.TopicId == topicId);
            list.Insert(0, firstItem);
            response.Result = list.ToArray(); // a bit of a hack :-D
        }
    }
}