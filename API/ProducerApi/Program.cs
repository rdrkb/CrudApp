using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        /*cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint("lol", e =>
        {
            // Configure the consumer to process messages from the queue
            e.Consumer<StudentInfoConsumer>();
        });*/
    });
});

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
