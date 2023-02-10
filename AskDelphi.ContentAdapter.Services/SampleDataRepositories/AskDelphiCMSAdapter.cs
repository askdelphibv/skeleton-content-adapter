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
        public static readonly string ProcessTopicTypeTitle = "Process";
        public static readonly string ProcessTopicNamespace = "http://tempuri.org/imola-process";
        public static readonly string TaskTopicTypeTitle = "Task";
        public static readonly string TaskTopicNamespace = "http://tempuri.org/imola-task";
        public static readonly string ImageTopicTypeTitle = "Image";
        public static readonly string ImageTopicNamespace = "http://tempuri.org/doppio-image";
        public static readonly string DefaultVersion = "1.0";

        public static readonly string ProcessTaskRelationPyramidLevelTitle = "Process";

        public async Task<Guid> GetRelationTypeKeyFor(IOperationContext operationContext, string processTaskRelationPyramidLevelTitle, string taskTopicTypeTitle, string taskTopicNamespace, string use, string view)
        {
            return await Task.FromResult(Guid.Empty);
        }
    }
}
