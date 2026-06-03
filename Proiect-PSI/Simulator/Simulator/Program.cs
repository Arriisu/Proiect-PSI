using Simulator;
using Simulator.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermiteBlazor", policy => 
        policy.WithOrigins("http://localhost:5062", "https://localhost:7260")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// Add services to the container.
builder.Services.AddSingleton<StareSistem>();
builder.Services.AddHostedService<LogicaM23>();
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseCors("PermiteBlazor");

app.MapControllers();

app.MapHub<M23Hub>("/m23hub");

app.Run();
