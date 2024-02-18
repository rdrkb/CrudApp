using System.Text;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
var mongoConnectionString = "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("MyDatabase");
builder.Services.AddSingleton(mongoDatabase);

// Redis configuration
var redisConnectionString = "localhost:6379";
var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton(redis);


// RabbitMQ configuration
builder.Services.AddMassTransit(config =>
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
builder.Services.AddScoped<StudentInfoConsumer>();


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<IAccountDataAccess, AccountDataAccess>();
builder.Services.AddSingleton<IStudentDataAccess, StudentDataAccess>();
builder.Services.AddSingleton<ITeacherDataAccess, TeacherDataAccess>();
builder.Services.AddSingleton<IAdminDataAccess, AdminDataAccess>();
builder.Services.AddSingleton<IMessageDataAccess,  MessageDataAccess>();

builder.Services.AddSingleton<IRedisCache,  RedisCache>();

builder.Services.AddSingleton<IAccountLogic, AccountLogic>();
builder.Services.AddSingleton<IStudentLogic, StudentLogic>();
builder.Services.AddSingleton<ITeacherLogic, TeacherLogic>();
builder.Services.AddSingleton<IAdminLogic, AdminLogic>();
builder.Services.AddSingleton<IMessageLogic,  MessageLogic>();

builder.Services.AddSingleton<ITokenService, TokenService>();


// Configuration
var configuration = builder.Configuration;
var jwtSecretKey = configuration["JwtSettings:SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("TeacherOnly", policy => policy.RequireRole("teacher"));
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("student"));
});

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseWebSockets();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.MapControllers();

// Run the application
app.Run();
