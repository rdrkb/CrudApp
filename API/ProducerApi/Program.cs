using Contracts.MongoClientFactory;
using MassTransit;
using NotificationApi.Business.Consumers;
using NotificationApi.Business.Notification;
using NotificationApi.Repository.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoClientFactory>();

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<UserInfoUpdatedEventConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMqConfig:HostName"], h =>
        {
            h.Username(builder.Configuration["RabbitMqConfig:UserName"]);
            h.Password(builder.Configuration["RabbitMqConfig:Password"]);
        });
        /*cfg.ReceiveEndpoint("notification_queue", ep =>
        {
            ep.ConfigureConsumer<UserInfoUpdatedEventConsumer>(context);
        });*/

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddScoped<UserInfoUpdatedEventConsumer>();

builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<INotificationRepository, NotificationRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddControllers();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
