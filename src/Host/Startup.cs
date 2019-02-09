using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Host.Data;
using Host.Models;
using Host.Configuration;
using Host.DataContext;
using Host.Business.IDbServices;
using Host.Business.DbServices;
using System;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using StackExchange.Profiling.Storage;
using Host.Helper;

namespace Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
                //Environment.GetEnvironmentVariable("DefaultConnection");
            Debug.Assert(connectionString != null, nameof(connectionString) + " != null");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<EcoDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

           

            // services Add 
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IEmployeeProfileService, EmployeeProfileService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<DownloadObservationPdf>();
            //services.AddScoped<QRCodeGenerator>();
            services.AddScoped<IBranchEmployeeService, BranchEmployeeService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IStationLocationService, StationLocationService>();
            services.AddScoped<IActivityTypeService, ActivityTypeService>();
            services.AddScoped<IActivityPerformService, ActivityPerformService>();
           // services.AddScoped<IAuditDbContext, AuditDbContext>();
           // services.AddScoped<IEfHepler, EfHepler>();
            services.AddScoped<IGraphService, GraphService>();
            services.AddScoped<IJsonDataService, JsonDataService>();
            services.AddScoped<IClientCompanyService, ClientCompanyService>();

            services.AddCors(options =>
            {
                options.AddPolicy("eco-report-grid",
                    builder => builder.WithOrigins("http://localhost:3000"));
            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryClients(Clients.Get())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                }
                )
                .AddGoogle(options =>
                {
                    options.ClientId = "998042782978-s07498t8i8jas7npj4crve1skpromf37.apps.googleusercontent.com";
                    options.ClientSecret = "HsnwJri_53zn7VcO1Fm7THBb";
                    options.SaveTokens = true;

                });

            services.AddMvc();


            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Eco App - EcoApp HTTP API",
                    Version = "v1",
                    Description = "The EcoApp Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                    TermsOfService = "Terms Of Service",
                });
            });


            //services.AddMiniProfiler(options =>
            //{
            //    // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

            //    // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
            //    options.RouteBasePath = "/profiler";

            //    // (Optional) Control storage
            //    // (default is 30 minutes in MemoryCacheStorage)
            //    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

            //    // (Optional) Control which SQL formatter to use, InlineFormatter is the default
            //    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

            //    //// (Optional) To control authorization, you can use the Func<HttpRequest, bool> options:
            //    //// (default is everyone can access profilers)
            //    //options.ResultsAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
            //    //options.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;

            //    //// (Optional)  To control which requests are profiled, use the Func<HttpRequest, bool> option:
            //    //// (default is everything should be profiled)
            //    //options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

            //    //// (Optional) Profiles are stored under a user ID, function to get it:
            //    //// (default is null, since above methods don't use it by default)
            //    //options.UserIdProvider = request => MyGetUserIdFunction(request);

            //    //// (Optional) Swap out the entire profiler provider, if you want
            //    //// (default handles async and works fine for almost all appliations)
            //    //options.ProfilerProvider = new MyProfilerProvider();

            //    // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
            //    // (defaults to true, and connection opening/closing is tracked)
            //    options.TrackConnectionOpenClose = true;
            //});

            // Other ConfigureServices() code...
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
                app.UseMiddleware<AuthenticationMiddleware>();

                app.UseMvc();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseMiniProfiler();

            app.UseStaticFiles();

            //app.UseAuthentication(); // UseAuthentication not needed -- UseIdentityServer add this
            app.UseIdentityServer();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors("eco-report-grid");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
