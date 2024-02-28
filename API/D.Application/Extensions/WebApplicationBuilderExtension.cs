using B.Database.MongoDB;
using B.Database.MongoDB.AccountData;
using B.Database.MongoDB.AdminData;
using B.Database.MongoDB.MessagedData;
using B.Database.MongoDB.StudentData;
using B.Database.MongoDB.TeacherData;
using B.Database.RedisCache;
using C.Business.Accounts;
using C.Business.Admins;
using C.Business.Messages;
using C.Business.Security;
using C.Business.Students;
using C.Business.Students.Consumers;
using C.Business.Teachers;
using D.Application.Websocket;
using MassTransit;
using MongoDB.Driver;
using StackExchange.Redis;

namespace D.Application.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static void AddMongoDB(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            
            services.AddSingleton<MongoClientFactory>();
        }

        public static void AddRabbitMQ(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMqConfig:HostName"], h =>
                    {
                        h.Username(builder.Configuration["RabbitMqConfig:UserName"]);
                        h.Password(builder.Configuration["RabbitMqConfig:Password"]);
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
