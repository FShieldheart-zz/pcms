using API.Helper.Classes;
using API.MappingProfile.Classes;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Repository.Context.Classes;
using Repository.UnitOfWork.Classes;
using Service.AccessPoint.Classes;
using Service.AccessPoint.Interfaces;
using Structure.Domain.Classes;
using Structure.Model.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace API
{
    public class Startup
    {
        private const string _configurationFilePath = "appSettings.json";

        private string _apiVersion;
        private string _apiTitle;
        private string _logPath;
        private string _corsPolicyName;
        private string[] _corsOrigins;
        private string[] _corsMethods;
        private string[] _corsHeaders;
        private string _swaggerEndpoint;
        private string _swaggerTitle;
        private string _swaggerDocumentTitle;

        public static string ProductInMemoryCacheKey { get; private set; }
        public static string CampaignInMemoryCacheKey { get; private set; }
        public static string ConnectionString { get; private set; }
        public static long CacheTimeoutMinute { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ReadConfiguration();
        }

        public IConfiguration Configuration { get; }

        private void ReadConfiguration()
        {
            string configurationString = File.ReadAllText(_configurationFilePath);
            dynamic configurationJson = JObject.Parse(configurationString);

            ConnectionString = configurationJson["SqliteConnectionString"].ToString();
            _logPath = configurationJson["LoggingPath"].ToString();
            ProductInMemoryCacheKey = configurationJson["Cache"].ProductName.ToString();
            CampaignInMemoryCacheKey = configurationJson["Cache"].CompanyName.ToString();
            CacheTimeoutMinute = long.Parse(configurationJson["Cache"].TimeoutMinute.ToString());
            _corsPolicyName = configurationJson["CORS"].PolicyName.ToString();
            _corsOrigins = configurationJson["CORS"].Origins.ToObject<string[]>();
            _corsMethods = configurationJson["CORS"].Methods.ToObject<string[]>();
            _corsHeaders = configurationJson["CORS"].Headers.ToObject<string[]>();
            _apiVersion = configurationJson["Swagger"].API.Version.ToString();
            _apiTitle = configurationJson["Swagger"].API.Title.ToString();
            _swaggerEndpoint = configurationJson["Swagger"].Endpoint.ToString();
            _swaggerTitle = configurationJson["Swagger"].Title.ToString();
            _swaggerDocumentTitle = configurationJson["Swagger"].DocumentTitle.ToString();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(_corsPolicyName, builder =>
            {
                builder.WithOrigins(_corsOrigins)
                       .WithMethods(_corsMethods)
                       .WithHeaders(_corsHeaders);
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_apiVersion, new Info { Title = _apiTitle, Version = _apiVersion });
            });

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();

            ReadConfiguration();

            services.AddScoped<IGeneric<Product>, Generic<Product>>();
            services.AddScoped<IGeneric<Campaign>, Generic<Campaign>>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IMainDbContext, MainDbContext>();
            services.AddSingleton<IDatabaseOptions, DatabaseOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(_corsPolicyName);

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(_swaggerEndpoint, _swaggerTitle);
                c.DocumentTitle = _swaggerDocumentTitle;
            });

            loggerFactory.AddFile(_logPath);
        }
    }
}
