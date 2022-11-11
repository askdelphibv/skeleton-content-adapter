﻿using AskDelphi.SampleContentAdapter.ServiceModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ITopicRepository, SampleDataRepositories.SampleDataTopicRepository>();
            services.AddSingleton<IResourceRepository, SampleDataRepositories.SampleDataResourceRepository>();
        }
    }
}
