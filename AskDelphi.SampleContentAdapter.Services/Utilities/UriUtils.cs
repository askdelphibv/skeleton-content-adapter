using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AskDelphi.SampleContentAdapter.Services.Utilities
{
    /// <summary>
    /// Utility for Uri operations
    /// </summary>
    public static class UriUtils
    {
        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// </summary>
        /// <param name="url">Target url</param>
        /// <param name="paramName">Name of the parameter to add.</param>
        /// <param name="paramValue">Value for the parameter to add.</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;

            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Gets query string param value for specified uri and parameter name
        /// Returns null if param does not exist on query string
        /// </summary>
        /// <param name="url">Target Url</param>
        /// <param name="paramName">Query string param name</param>
        /// <returns></returns>
        public static string GetQueryParam(this Uri url, string paramName)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            return query[paramName];
        }
    }
}
