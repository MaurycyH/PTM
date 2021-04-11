using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTM.Logic;
using PTM.Services.TaskBoards;
using PTM.Services.Users;
using PTM.Services.WorkItemCollections;
using PTM.Services.WorkItems;

namespace PTM.Services
{
    public class Startup
    {
        /// <summary>
        /// Argumenty wejœciowe serwisów
        /// </summary>
        public static string[] Args { get; set; } = new string[] { };

        /// <summary>
        /// Konfiguracja serwisów
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Domyœlny ctor.
        /// </summary>
        /// <param name="env">Œrodowisko uruchomieniowe</param>
        /// <param name="loggerFactory">Fabryka logowania</param>
        public Startup(IWebHostEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddDbContext<PtmModel>();
            services.AddScoped<IDatabaseContext>(provider => provider.GetService<PtmModel>());
            services.AddScoped<ITaskBoardRepository, TaskBoardRepository>();
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            services.AddScoped<IWorkItemCollectionsRepository, WorkItemCollectionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
