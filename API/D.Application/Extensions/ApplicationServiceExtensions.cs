using B.Database.MongoDB.AccountData;
using B.Database.MongoDB.AdminData;
using B.Database.MongoDB.MessagedData;
using B.Database.MongoDB.StudentData;
using B.Database.MongoDB.TeacherData;
using B.Database.RedisCache;
using C.Business.AccountLogic;
using C.Business.AdminLogic;
using C.Business.Consumer;
using C.Business.MessageLogic;
using C.Business.StudentLogic;
using C.Business.TeacherLogic;
using C.Business.TokenServices;
using MassTransit;
using MongoDB.Driver;
using StackExchange.Redis;

namespace D.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddMongoDB(this IServiceCollection services)
        {
            // var mongoConnectionString = configuration.GetConnectionString("MongoDB");
            var mongoConnectionString = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(mongoConnectionString);
            // var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);
            var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
            services.AddSingleton(mongoDatabase);
        }

        public static void AddRedisCache(this IServiceCollection services)
        {
            // var redisConnectionString = configuration.GetConnectionString("Redis");
            var redisConnectionString = "localhost:6379";
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            services.AddSingleton(redis);
        }

        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(config  =>
            {
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
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
            services.AddSingleton<IAccountDataAccess, AccountDataAccess>();
            services.AddSingleton<IStudentDataAccess, StudentDataAccess>();
            services.AddSingleton<ITeacherDataAccess, TeacherDataAccess>();
            services.AddSingleton<IAdminDataAccess, AdminDataAccess>();
            services.AddSingleton<IMessageDataAccess, MessageDataAccess>();

            // Cache
            services.AddSingleton<IRedisCache, RedisCache>();

            // Business Logic
            services.AddSingleton<IAccountLogic, AccountLogic>();
            services.AddSingleton<IStudentLogic, StudentLogic>();
            services.AddSingleton<ITeacherLogic, TeacherLogic>();
            services.AddSingleton<IAdminLogic, AdminLogic>();
            services.AddSingleton<IMessageLogic, MessageLogic>();

            // Token Service
            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
