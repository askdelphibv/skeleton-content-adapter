using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskDelphi.ContentAdapter.ServiceModel;
using AskDelphi.ContentAdapter.Services;
using AskDelphi.ContentAdapter.Services.Cache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AskDelphi.ContentAdapter
{
    /// <summary></summary>
    public class Startup
    {
        private const string CorsAllowSpecificOriginsPolicy = "_corsAllowSpecificOriginsPolicy";

        /// <summary></summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary></summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary></summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers((options) =>
            {
                options.Filters.Add<Filters.GlobalRequestFilter>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AskDelphi Sample Content Adapter",
                    Version = "v1",
                    Description = "Content adapter",
                    Contact = new OpenApiContact
                    {
                        Name = "AskDelphi B.V.",
                        Email = "mailto:support@askdelphi.com",
                        Url = new Uri("https://askdelphi.com"),
                    },
                    // TermsOfService = new Uri("https://askdelphi.com/assets/terms-of-service.html"),
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Need explicit permission",
                    //    Url = new Uri("https://askdelphi.com/assets/api-license.html"),
                    //}
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                          },
                          new string[] {}

                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsAllowSpecificOriginsPolicy,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                  });
            });

            services.AddLogging();
            services.AddMemoryCache();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

            services.AddTransient<IOperationContextFactory, OperationContextFactory>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddHostedService<HostedServices.CacheSynchronizationService>();

            services.AddServices();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((options) =>
            {
                string cfgSecret = Configuration.GetValue<string>("JWT:Secret");
                string cfgIssuer = Configuration.GetValue<string>("JWT:Issuer");
                string cfgAudience = Configuration.GetValue<string>("JWT:Audience");
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(cfgSecret));

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudiences = new string[] { 
                        cfgAudience
                    },
                    ValidIssuers = new string[] {
                        cfgIssuer
                    }
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary></summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AskDelphi.ContentAdapter v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(CorsAllowSpecificOriginsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
