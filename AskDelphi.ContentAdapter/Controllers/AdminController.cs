using AskDelphi.ContentAdapter.DTO;
using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.ServiceModel.DTO;
using AskDelphi.ContentAdapter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AskDelphi.ContentAdapter.Controllers
{
    /// <summary>
    /// For interacting with resources.
    /// </summary>
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<ContentController> logger;

        /// <summary>
        /// 
        /// </summary>    
        public AdminController(ILogger<ContentController> logger)
        {
            this.logger = logger;
        }
    }
}
