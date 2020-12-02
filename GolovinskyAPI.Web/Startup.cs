using Autofac;
using AutoMapper;
using GolovinskyAPI.Data;
using GolovinskyAPI.Logic.Handlers;
using GolovinskyAPI.Logic.Interfaces;
using GolovinskyAPI.Logic.Profiles;
using GolovinskyAPI.Logic.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using GolovinskyAPI.Data.Repositories;
using GolovinskyAPI.Data.Interfaces;
using Microsoft.Extensions.Hosting;

namespace GolovinskyAPI.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostEnvironment _env; 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var origins = Configuration.GetSection("CorsOrigins").GetChildren().ToArray().Select(c => c.Value).ToArray();
            var result = Configuration.GetSection("AuthService").GetChildren();
            var webRoot = _env.ContentRootPath;
            Global.Connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
            }));
            services.AddTransient<IBackgroundRepository, BackgroundRepository>(provider => new BackgroundRepository(Global.Connection));
            services.AddTransient<IAuthHandler, AuthHandler>();
            services.AddTransient<IUploadPicture, UploadPictureHandler>(upload => new UploadPictureHandler(_env));
            services.AddAutoMapper(typeof(AdminProfile), typeof(CatalogProfile), typeof(PictureProfile), typeof(BackgroundProfile));
            services.AddOptions();
            services.AddMvc();
            services.AddControllers().AddNewtonsoftJson();  
            services.Configure<AuthServiceModel>(Configuration.GetSection("AuthService"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo { Title = "Test Api", Description = "Swagger Test Api" });
                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"GolovinskyAPI.xml";
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata=false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = result.Where(x => x.Key == "Issuer").FirstOrDefault().Value,
                        ValidateAudience = false,
                        ValidAudience = result.Where(x => x.Key == "Audience").FirstOrDefault().Value,
                        ValidateLifetime = true,
                        IssuerSigningKey = GetSymmetricSecurityKey(result.Where(x => x.Key == "Key").FirstOrDefault().Value),
                        ValidateIssuerSigningKey = true 
                    }; 
                });                      
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseRouting(); 
            app.UseCors("ApiCorsPolicy");
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "Test Api");
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "Images")),

                RequestPath = "/mainimages"

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "AccountImages")),

                RequestPath = "/accountImages"

            });
        }
    }
}