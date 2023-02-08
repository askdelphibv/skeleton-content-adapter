using AskDelphi.SampleContentAdapter.ServiceModel;
using System;

namespace AskDelphi.SampleContentAdapter.DTO
{
    /// <summary>
    /// Indicates the utc date when the content was last changed
    /// </summary>
    public class ContentLastChangedDateResponse : APIResponseBase
    {
        /// <summary></summary>
        public ContentLastChangedDateResponse(IOperationContext operationContext) : base(Constants.APIVersion1, operationContext) { }

        /// <summary>
        /// Indicates the utc date when the content was last changed
        /// </summary>
        public DateTimeOffset? LastChangedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int Initialize(SCR<DateTimeOffset?> scr)
        {
            if (!scr.IsError)
            {
                LastChangedDate = scr.Result;
            }
            return base.InitializeFromSCR(scr);
        }
    }
}
