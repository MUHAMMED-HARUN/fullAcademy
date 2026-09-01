using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Application.Behaviors;
using System.Application.Services.Authentication.Login.Command;
using System.Application.Services.Authentication.Login.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Application
{
    public static class  ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            try
            {

                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly);
                });

                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

                services.AddAutoMapper(typeof(LoginProfile).Assembly);


                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

                return services;
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var e in ex.LoaderExceptions)
                {
                    Console.WriteLine("----");
                    Console.WriteLine(e.GetType().FullName);
                    Console.WriteLine(e.Message);
                }
                throw;
            }
      
    }
    }
}
