using System;
using System.IO;
using System.Reflection;
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
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Smart Fridge API",
                    Description = "An API for SmartFridgeApp.",
                    Contact = new Contact
                    {
                        Name = "Dariusz Koziol",
                        Email = "dariusz151@gmail.com"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

           

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
            app.UseSwagger();
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

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Fridge API");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
