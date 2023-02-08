using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AskDelphi.ContentAdapter.ServiceModel
{
    /// <summary>
    /// Service call result
    /// </summary>
    /// <typeparam name="T">Requested data type</typeparam>
    public class SCR<T>
    {
        /// <summary>
        /// The resulting data
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// True if an error is returned, false is a result is returned
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// HTTP status code that best identifies the response.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Error code that can be used to look up the error.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Error message that can eb used to look up any and all errors.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Instantiate the result from data.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SCR<T> FromData(T data) => new SCR<T> { IsError = false, Result = data, HttpStatusCode = HttpStatusCode.OK };

        /// <summary>
        /// Instantiate the result from an error situation.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static SCR<T> FromError(string errorCode, string errorMessage, HttpStatusCode statusCode) => new SCR<T> { IsError = true, ErrorCode = errorCode, ErrorMessage = errorMessage, HttpStatusCode = statusCode };
    }
}
