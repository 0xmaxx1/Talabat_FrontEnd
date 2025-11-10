using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationsServicesExtension
    {


        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Apply Dependency Injection for All Repositories
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Apply Dependency Injection for UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>(); // Per Request

            // Apply Dependency Injection For AutoMapper
            services.AddAutoMapper(typeof(MappingProfiles).Assembly); // Use Assembly if we use custom resolver [Inject IConfiguration]
            //services.AddAutoMapper(M => M.AddProfile(new MappingProfiles())); // Use this if we don't use custom resolver [Not Inject IConfiguration]

            // Apply Dependency Injection for Product Picture Url Resolver
            services.AddScoped<ProductPictureUrlResolver>();


            // Apply Configurations for API Validation Errors
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                { // ModelState  : Dictionary
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();

                    ApiValidationErrorResponse validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });


            // Apply Dependency Injection for Basket Repository
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            // Apply Dependency Injection for Order Services
            services.AddScoped<IOrderService, OrderService>();

            // Apply Dependency Injection for Payment Services
            services.AddScoped<IPaymentService, PaymentService>();

            // Apply Dependency Injection for Caching Service
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            return services;
        }


    }
}
