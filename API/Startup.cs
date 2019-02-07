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
        private static readonly string _configurationFilePath = "appSettings.json";
        private string _logPath;
        private static string[] _corsOrigins;
        private static string[] _corsMethods;
        private static string[] _corsHeaders;

        public static readonly string ProductInMemoryCacheKey = "pcms-imck-product";
        public static readonly string CampaignInMemoryCacheKey = "pcms-imck-campaign";
        public static string ConnectionString { get; private set; }
        public static long CacheTimeoutMinute { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ReadConfiguration()
        {
            string configurationString = File.ReadAllText(_configurationFilePath);
            dynamic configurationJson = JObject.Parse(configurationString);
            ConnectionString = configurationJson["SqliteConnectionString"].ToString();
            _logPath = configurationJson["LoggingPath"].ToString();
            CacheTimeoutMinute = long.Parse(configurationJson["CacheTimeoutMinute"].ToString());
            _corsOrigins = configurationJson["CORS"].Origins.ToObject<string[]>();
            _corsMethods = configurationJson["CORS"].Methods.ToObject<string[]>();
            _corsHeaders = configurationJson["CORS"].Headers.ToObject<string[]>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("PCMSPolicy", builder =>
            {
                builder.WithOrigins(_corsOrigins)
                       .WithMethods(_corsMethods)
                       .WithHeaders(_corsHeaders);
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "PCMS API", Version = "v1" });
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

            app.UseCors("PCMSPolicy");

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PCMS API V1");
                c.DocumentTitle = "PCMS API Dynamic Documentation";
            });

            loggerFactory.AddFile(_logPath);
        }
    }
}
