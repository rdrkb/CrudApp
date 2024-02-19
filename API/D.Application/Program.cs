using D.Application.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// MongoDB configuration
builder.Services.AddMongoDB();

// Redis configuration
builder.Services.AddRedisCache();

// RabbitMQ configuration
builder.Services.AddRabbitMQ();

builder.Services.AddCustomServices();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();


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

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.MapControllers();

// Run the application
app.Run();
