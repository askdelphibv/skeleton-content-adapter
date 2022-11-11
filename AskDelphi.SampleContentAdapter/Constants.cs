using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter
{
    /// <summary></summary>
    public static class Constants
    {
        /// <summary></summary>
        public const string APIVersion1 = "1";
        /// <summary></summary>
        public const bool Success = true;
        /// <summary></summary>
        public const bool Fail = false;

        /// <summary></summary>
        public const string ClaimTypeSystemID = "http://tempuri.org/askdelphi/remote-system-id";
        /// <summary></summary>
        public const string ClaimTypeTenant = "http://tempuri.org/askdelphi/tenant-id";
        /// <summary></summary>
        public const string ClaimTypeProject = "http://tempuri.org/askdelphi/project-id";

        /// <summary></summary>
        public const string ErrorFailedCode = "ERR_FAIL";
        /// <summary></summary>
        public const string ErrorFailedMessage = "The operation failed to complete";

        /// <summary></summary>
        public const string ErrorInternal = "ERR_INT";
        /// <summary></summary>
        public const string ErrorInternalMessage = "An internal error occurred";

        /// <summary></summary>
        public const string ErrorInvalidCredentialsCode = "ERR_PERM";
        /// <summary></summary>
        public const string ErrorInvalidCredentialsMessage = "Invalid credentials";

        /// <summary></summary>
        public const string ErrorNotSupportedCode = "ERR_SUPP";
        /// <summary></summary>
        public const string ErrorNotSupportedMessage = "Operation is not supported";
    }
}
