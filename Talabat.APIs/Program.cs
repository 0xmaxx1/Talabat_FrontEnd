
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Models.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        // Entry Point
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers(); // Allow Dependency Injection for API Services


            // Get built-in Swagger Extensions
            builder.Services.AddSwaggerServices();


            // Apply Dependency Injection for DbContext
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    options => options.EnableRetryOnFailure());
            });


            // Apply Dependency Injection for Identity
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });


            // Apply Dependency Injection for Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                string? connection = builder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connection);
            });


            // Apply built-in Application Services
            builder.Services.AddApplicationServices();
            //ApplicationsServicesExtension.AddApplicationServices(builder.Services);


            // Apply Identity Services
            builder.Services.AddIdentityServices(builder.Configuration);

            // Apply Cors Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", corsOptions =>
                {
                    corsOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });

            #endregion


            var app = builder.Build();


            #region Configurations for automatic Migrations
            // Container for Scoped Services
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>(); // Create a new instance of ILoggerFactory

            try
            {
                // Ask Explicitly for the DbContext
                var dbContext = services.GetRequiredService<StoreContext>();// Create a new instance of DbContext

                await dbContext.Database.MigrateAsync(); // Apply Migrations Automatically [Update Database]


                await StoreContextSeed.SeedAsync(dbContext); // Seed the Database with Data


                var identityContext = services.GetRequiredService<AppIdentityDbContext>(); // Create a new instance of Identity DbContext
                await identityContext.Database.MigrateAsync(); // Apply Migrations Automatically [Update Database]

                // Seed the Identity Database with Data
                UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager); // Seed the Identity Database with Data
            }
            catch (Exception Ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(Ex, Ex.Message);
            }
            #endregion


            #region Configure Kestrel Middlewares

            // Apply Custom Middleware for Exception Handling
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Apply built-in Swagger Middlewares
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // ReExecute the URL with the StatusCode without Redirection [One Request]
            //app.UseStatusCodePagesWithRedirects("/errors/{0}"); // Redirect the URL with the StatusCode [Two Requests]

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");


            app.MapControllers(); // To allow Routing for APIs
            #endregion


            app.Run();
        }
    }
}
