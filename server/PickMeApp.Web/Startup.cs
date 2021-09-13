using iComplyICO.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PickMeApp.Application.Models;
using PickMeApp.Application.Services;
using PickMeApp.Core.Models;
using PickMeApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PickMeApp.Web.Hubs;
using PickMeApp.Web.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using PickMeApp.Web.Middlewares;

namespace PickMeApp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connString = Configuration["DefaultConnection"];
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));

            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(connString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Authentication JWT token 
            var jwtSettings = new JwtSettings()
            {
                Secret = Configuration["JWTSecretKey"],
                TokenLifetime = new TimeSpan(0, 0, 20, 0, 0) // 20 minutes
            };
            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false,
                ValidIssuer = "pickmeapp.com",
                ValidAudience = "api.pickmeapp.com",
                ValidateActor =false,
                ValidateTokenReplay = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSecretKey"])),
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //sharedOptions.DefaultScheme = "icomplyAuth";
                //sharedOptions.DefaultChallengeScheme = "icomplyAuth";
            })
            //.AddPolicyScheme("icomplyAuth", "Authorization Bearer or ApiKey", options =>
            //{
            //    options.ForwardDefaultSelector = context =>
            //    {
            //        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            //        if (authHeader?.StartsWith("Bearer ") == true)
            //        {
            //            return JwtBearerDefaults.AuthenticationScheme;
            //        }
            //        return JwtBearerDefaults.AuthenticationScheme;
            //    };
            //})
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;

                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var accessToken = context.Request.Query["access_token"];

                //        var path = context.HttpContext.Request.Path;
                //        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notifications")))
                //        {
                //            context.Token = accessToken;
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

                //options.RequireHttpsMetadata = false;
                //options.SaveToken = true;
                //options.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateIssuerSigningKey = false,
                //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSecretKey"])),
                //    ValidateIssuer = false,
                //    ValidateAudience = false,
                //};
            });

            //services.AddControllers();

            services.AddControllers(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder(new[] {
                            JwtBearerDefaults.AuthenticationScheme }).RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(defaultPolicy));

                // options.InputFormatters.Add()
            });//.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            
            //services.AddControllers(setupAction =>
            //{
            //    setupAction.ReturnHttpNotAcceptable = true;
            //    setupAction.CacheProfiles.Add("240SecondsCacheProfile",
            //                                    new CacheProfile()
            //                                    {
            //                                        Duration = 240
            //                                    });

            //    var defaultPolicy = new AuthorizationPolicyBuilder(new[] {
            //                JwtBearerDefaults.AuthenticationScheme }).RequireAuthenticatedUser().Build();
            //    setupAction.Filters.Add(new AuthorizeFilter(defaultPolicy));
            //}).AddNewtonsoftJson(setupAction =>
            //{
            //    setupAction.SerializerSettings.ContractResolver =
            //       new CamelCasePropertyNamesContractResolver();
            //})
            //.AddXmlDataContractSerializerFormatters()
            //.ConfigureApiBehaviorOptions(setupAction =>
            //{
            //    setupAction.InvalidModelStateResponseFactory = context =>
            //    {
            //        var problemDetails = new ValidationProblemDetails(context.ModelState)
            //        {
            //            Type = "https://pick-me-up.com/modelvalidationproblem",
            //            Title = "One or more model validation errors occurred.",
            //            Status = StatusCodes.Status422UnprocessableEntity,
            //            Detail = "See the errors property for details.",
            //            Instance = context.HttpContext.Request.Path
            //        };

            //        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            //        return new UnprocessableEntityObjectResult(problemDetails)
            //        {
            //            ContentTypes = { "application/problem+json" }
            //        };
            //    };
            //});

            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            //services.AddCors(options => options.AddPolicy("CorsPolicy",
            //    builder =>
            //    {
            //        builder.AllowAnyMethod()
            //               .AllowAnyHeader()
            //               .WithOrigins("http://localhost:3000")
            //               .AllowCredentials();
            //    }));

            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddTransient<IAuthService, AuthService>();
            // register RideRepository
            services.AddTransient<IRideRepository, RideRepository>();
            services.AddTransient<IRideService, RideService>();

            // register PropertyMappingService
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            // register PropertyCheckerService
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

            // Register AutoMapper profiles
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pick Me App API", Version = "v1" });
            //    c.EnableAnnotations();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();


            // app.UseCors("CorsPolicy");

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<NotificationsHub>("/notifications");
            //});

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationsHub>("/notifications");
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllers();
            });

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();
            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pick Me App API V1"));
        }
    }
}
