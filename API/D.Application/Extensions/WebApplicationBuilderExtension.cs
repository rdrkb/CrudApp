using Business;
using Business.Accounts.Repositories;
using Business.Accounts.Services;
using Business.Admins;
using Business.Messages;
using Business.Redis;
using Business.Security;
using Business.Students;
using Business.Students.Consumers;
using Business.Teachers;
using MassTransit;
using SchoolManagementApi.Websocket;
using Business.Extensions;

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
                  
                    // Register the consumer
                    cfg.ReceiveEndpoint("my_queue", ep =>
                    {
                        ep.Consumer<StudentInfoConsumer>(context);
                    });
                });
            });
            services.AddScoped<StudentInfoConsumer>();
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            // DataAccess
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IStudentRepository, StudentCacheRepositoryDecorator>();
            services.AddSingleton<StudentRepository>();
            services.AddSingleton<ITeacherRepository, TeacherRepository>();
            services.AddSingleton<IAdminRepository, AdminRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();

            // Cache
            services.AddSingleton<IRedisCache, RedisCache>();

            // Business Logic
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<ITeacherService, TeacherService>();
            services.AddSingleton<IAdminService, AdminService>();
            services.AddSingleton<IMessageService, MessageService>();

            // Token Service
            services.AddSingleton<ITokenService, TokenService>();

            // WebSocket middleware
            services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
        }
    }
}
