using EchoesOfUzbekistan.Api.Extensions;
using EchoesOfUzbekistan.Application;
using EchoesOfUzbekistan.Infrastructure;
using Microsoft.AspNetCore.Authentication;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Temporary cors settings to test the front-end application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins(new string[] { "http://localhost:4200", "http://172.26.112.1:5000/" }) // Allow requests from this origin
                  .AllowAnyHeader()                     // Allow any headers
                  .AllowAnyMethod();                    // Allow any HTTP methods
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}
app.UseCors("AllowSpecificOrigin");

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
