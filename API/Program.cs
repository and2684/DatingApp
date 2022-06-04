using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// CORS - это защитны механизм, позволяющий запускать наше приложение с других хостов
// (например у нас api нас localhost:5001 и мы его не сможем запустить с ангуляра, который на localhost:4200 пока не включим CORS)
builder.Services.AddCors();
builder.Services.AddLogging(); // Add MAS 25.05.2022

// Add MAS 31.05.2022
// Вынесли все включения сервисов в папку Extenstions
// Сервисы приложения (EF core, создание токенов и пр.)
builder.Services.AddApplicationServices(builder.Configuration);
// Сервисы аутентификации
builder.Services.AddIdentityServices(builder.Configuration);
// End Add MAS 31.05.2022

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));  

app.UseAuthentication(); // Add MAS 31.05.2022
app.UseAuthorization();

app.MapControllers();

app.Run();
