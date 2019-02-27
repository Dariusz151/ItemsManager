using System.IO;
using ItemsManager.Common.Auth;
using ItemsManager.FoodItems.Domain.Repositories;
using ItemsManager.FoodItems.Repositories;
using ItemsManager.Recipes.Repositories;
using ItemsManager.Users.Domain.Repositories;
using ItemsManager.Users.Domain.Services;
using ItemsManager.Users.Repositories;
using ItemsManager.Users.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ItemsManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();
            services.AddJwt(Configuration);
            
            services.AddScoped<IFoodItemsRepository, FoodItemsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRecipesRepository, RecipesRepository>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddSingleton<IEncrypter, Encrypter>();
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                          name: "default",
                          template: "{controller=Login}/{action=Login}/{id?}");
            });

            app.UseAuthentication();
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/static")),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
