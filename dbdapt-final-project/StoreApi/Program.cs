using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StoreLib.Contexts;
using StoreLib.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Добавление контекста БД
builder.Services.AddDbContext<StoreDbContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// секретный ключ для jwt-токена (из статическоо свойства сервиса)
string _secretKey = AuthService.SecretKey;

//Добавляет авторизацию черз jwt bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = true,

            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
