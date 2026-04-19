using Application.Common;
using Application.Common.Abstractions;
using Application.Common.Validation;
using Application.System.MasterData.Company.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
             services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
 
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IValidationMessages, ValidationMessages>();
            return services;
        }
    }
}