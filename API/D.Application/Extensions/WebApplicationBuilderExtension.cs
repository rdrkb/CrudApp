using Business.Accounts.Repositories;
using Business.Accounts.Services;
using Business.Admins;
using Business.Security;
using Business.Teachers;
using MassTransit;
using Business.Extensions;
using Database;
using Database.Redis;
using Contracts.MongoClientFactory;
using Business.Students.Services;
using Business.Students.Repositories;
using SchoolManagement.Shared.CQRS;
using Business.Accounts.Commands;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagementApi.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static void AddMongoDB(this IServiceCollection services)
        {
            services.AddSingleton<MongoClientFactory>();
        }

        public static void AddMediatRService(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            services.AddSchoolBusinesServices();
        }

        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMqConfig:HostName"], h =>
                    {
                        h.Username(configuration["RabbitMqConfig:UserName"]);
                        h.Password(configuration["RabbitMqConfig:Password"]);
                    });
                });
            });
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ICommandService, CommandService>();
             services.AddKeyedTransient<IDynamicCommandConsumer, UserLoginDynamicCommandConsumer>("UserLoginDynamicCommandConsumer");
            // DataAccess
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IStudentRepository, StudentCacheRepositoryDecorator>();
            services.AddSingleton<StudentRepository>();
            services.AddSingleton<ITeacherRepository, TeacherRepository>();
            services.AddSingleton<IAdminRepository, AdminRepository>();
            

            // Cache
            services.AddSingleton<IRedisCache, RedisCache>();

            // Business Logic
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<ITeacherService, TeacherService>();
            services.AddSingleton<IAdminService, AdminService>();
            

            // Token Service
            services.AddSingleton<ITokenService, TokenService>();

            
        }

        public static void AddAllDynamicCommandConsumers(this IServiceCollection services)
        {
            typeof(UserDeleteCommand).Assembly.GetExportedTypes().ToList().ForEach(type =>
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsAssignableTo(typeof(IDynamicCommandConsumer)))
                {
                    services.AddKeyedTransient(typeof(IDynamicCommandConsumer), type.Name, type);
                }
            });
                  
        }
    }
}
