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
        public static readonly string ProcessTaskRelationPyramidLevelTitle = "Task";
        public static readonly Guid ProcessTaskRelationType = new Guid("7019e146-6b57-42e5-afe1-0459615d49c2");

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
