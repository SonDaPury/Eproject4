using backend.Base;
using backend.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace backend.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<LMSContext>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = acttionContext =>
                {
                    var errors = acttionContext.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();
                    var errorResponse = new APIValidationError
                    {
                        Errors = errors,
                        Message = "Validation failed"
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}
