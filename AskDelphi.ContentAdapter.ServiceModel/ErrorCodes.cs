using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.ServiceModel
{
    public static class ErrorCodes
    {
        public const string TopicRepositoryFileNotFound = "TR_404";
        public const string TopicRepositoryFileNotFoundMessage = "File or path to file was not found or not accessible";

        public const string ResourceRepositoryFileNotFound = "RR_404";
        public const string ResourceRepositoryFileNotFoundMessage = "File or path to file was not found or not accessible";

        public const string TopicRepositoryDataNotFound = "TR_NODATA";
        public const string TopicRepositoryDataNotFoundMessage = "Data not found or not accessible";

        public const string ArisExportDownloadError = "AEDE_FAILED";
        public const string ArisExportDownloadErrorMessage = "Could not download remote file";

        public const string TopicRepositoryGetContentError = "TRCCE";
        public const string TopicRepositoryGetContentErrorMessage = "Could not get the topic content";

        public const string TopicRepositoryGetMetadataError = "TRGME";
        public const string TopicRepositoryGetMetadataErrorMessage = "Could not get the topic metadata";

        public const string AzureFileServiceGetResourceStreamError = "AFSGSE";
        public const string AzureFileServiceGetResourceStreamErrorMessage = "Failed to download resource from azure storage";


    }
}
