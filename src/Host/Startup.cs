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
using Swashbuckle.AspNetCore.Swagger;
using System;
using Host.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace Host
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
            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

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
            services.AddScoped<QRCodeGenerator>();
            services.AddScoped<IBranchEmployeeService, BranchEmployeeService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IStationLocationService, StationLocationService>();
            services.AddScoped<IActivityTypeService, ActivityTypeService>();
            services.AddScoped<IActivityPerformService, ActivityPerformService>();
            services.AddScoped<IAuditDbContext, AuditDbContext>();
            services.AddScoped<IEfHepler, EfHepler>();
            services.AddScoped<IGraphService, GraphService>();

            services.AddMvc();

            


            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryClients(Clients.Get())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication()
                .AddJwtBearer(cfg => {

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

            app.UseStaticFiles();

            //app.UseAuthentication(); // UseAuthentication not needed -- UseIdentityServer add this
            app.UseIdentityServer();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
