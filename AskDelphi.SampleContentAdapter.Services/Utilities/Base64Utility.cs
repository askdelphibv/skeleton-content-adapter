using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services.Utilities
{
    public static class Base64Utility
    {
        /// <summary>
        /// Gets the base64 encoded string after the comma
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string GetEncodedStringFromBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                return string.Empty;
            }
            Regex regex = new Regex(",(.*)");
            var v = regex.Match(base64);
            string data = v.Groups[1].ToString();
            return data;
        }

        /// <summary>
        /// Gets the content type inbetween the colon and semi-colon
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string GetContentTypeFromBase64(string base64)
        {            
            if(string.IsNullOrEmpty(base64))
            {
                return string.Empty;
            }
            Regex regex = new Regex(":(.*);");
            var v = regex.Match(base64);
            string type = v.Groups[1].ToString();
            return type;
        }
    }
}
