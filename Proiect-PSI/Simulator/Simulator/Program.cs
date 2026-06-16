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

builder.Services.AddSingleton<StareSistem>();
builder.Services.AddHostedService<LogicaM23>();

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

app.UseCors("PermiteBlazor");

app.UseAuthorization();
app.MapControllers();
app.MapHub<M23Hub>("/m23hub");

app.Run();