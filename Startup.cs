using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SimpleCrudAPI.Models;
using SimpleCrudAPI.Repositories;
using SimpleCrudAPI.Security;
using SimpleCrudAPI.Security.Action_Filters;

namespace SimpleCrudAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            string firebaseProjectName = Configuration.GetValue<string>("Firebase:Project_Name");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://securetoken.google.com/{firebaseProjectName}";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectName}",
                            ValidateAudience = true,
                            ValidAudience = firebaseProjectName,
                            ValidateLifetime = true
                        };
                    });


            services.AddEntityFrameworkSqlite().AddDbContext<SimpleCrudDBContext>(options => { options.UseSqlite($"Data Source={Environment.ContentRootPath}\\SimpleCrudDB.db"); });

            services.AddSingleton<IFirebaseAuthProvider>(new FirebaseAuthProvider(new FirebaseConfig(Configuration.GetValue<string>("Firebase:API_Key"))));

            // Dependency Injection
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFirebaseRepository, FirebaseRepository>();
            services.AddScoped<ISecurityHelper, SecurityHelper>();

            // Custom Action Filters
            services.AddScoped<AdminValidation>();
            services.AddScoped<UserOwnerValidation>();
            services.AddScoped<ReceiptOwnerValidation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
            );

            ConfigureFirebaseAppInstance();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureFirebaseAppInstance()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(Configuration.GetValue<string>("Firebase:Private_Key"))
            });
        }
    }
}
