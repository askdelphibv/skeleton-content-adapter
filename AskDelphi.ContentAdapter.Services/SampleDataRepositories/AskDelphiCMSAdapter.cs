using AskDelphi.ContentAdapter.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Services.SampleDataRepositories
{

    /// <summary>
    /// This class exists as a layer between the askdelphi cms and the adapter code.
    /// </summary>
    public class AskDelphiCMSAdapter: ICMSAdapter
    {
        /// <summary>
        /// This data is all copied from the project's content design.
        /// If using multiple projects, make an array poer project, the project ID should be in the 
        /// /operation context (it is read from the claims)
        /// </summary>
        public static readonly string ProcessTopicTypeTitle = "Skill area";
        public static readonly string ProcessTopicNamespace = "http://tempuri.org/imola-skill-area";
        public static readonly string TaskTopicTypeTitle = "Task";
        public static readonly string TaskTopicNamespace = "http://tempuri.org/imola-task";
        public static readonly string ImageTopicTypeTitle = "Image";
        public static readonly string ImageTopicNamespace = "http://tempuri.org/doppio-image";
        public static readonly string ExternalContentTopicNamespace = "http://tempuri.org/doppio-external";
        public static readonly string ExternalContentTopicTypeTitle = "External URL";

        public static readonly string ProcessTaskRelationPyramidLevelTitle = "Task";
        // Update this guid from the content design, this is the Relation Type Guid (topic type process, relation type task at task PL)
        public static readonly Guid ProcessTaskRelationType = new Guid("2edaa270-751d-4723-89e5-0825c2b4a650");

        public static readonly string TaskURLPyramidLevelTitle = "Reference";
        // Update this guid from the content design, this is the Relation Type Guid (topic type task, relation type Reference at Reference PL)
        public static readonly Guid TaskURLRelationType = new Guid("57e2f985-0651-42ee-b979-3c536d959e4c");

        public static readonly string DefaultVersion = "1.0";

        public async Task<Guid> GetRelationTypeKeyFor(IOperationContext operationContext,
            string processTaskRelationPyramidLevelTitle,
            string taskTopicTypeTitle,
            string taskTopicNamespace,
            string use,
            string view)
        {
            Guid projectGuid = operationContext.GetProjectGuid() ?? Guid.Empty;
            // TODO: Should use this project guid to look up the appropriate names, can regsiter them somehow.
            return await Task.FromResult(ProcessTaskRelationType);
        }
    }
}
