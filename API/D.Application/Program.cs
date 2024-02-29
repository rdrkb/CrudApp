using SchoolManagementApi.Extensions;
using SchoolManagementApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// MongoDB configuration
builder.Services.AddMongoDB();

// RabbitMQ configuration
builder.Services.AddRabbitMQ(builder.Configuration);

// MediatR configuration
builder.Services.AddMediatRService();

builder.Services.AddCustomServices();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();


builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseWebSockets();
app.Map("/ws", builder =>
{
    builder.UseMiddleware<WebSocketMiddleware>();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// Run the application
app.Run();
