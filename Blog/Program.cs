using Blog.Application.Services.Implementations;
using Blog.Application.Services.Interfaces;
using Blog.Domain.Models;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
//var connectionString = builder.Configuration.GetConnectionString("MySQLConnectionString");
var connectionString = "Server = localhost; DataBase = blog_db; Uid = root; Pwd = admin123";

builder.Services.AddDbContext<MySqlBlogContext>(options =>
    options.UseMySql(connectionString,
    new MySqlServerVersion(new Version(8, 4, 3)),
    mysqlOptions => mysqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
));


/// Configuração de identidade
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<MySqlBlogContext>()
    .AddDefaultTokenProviders();

// Configuração de autenticação JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// Adicionando serviços
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "Blog", Version = "v1" }));


var app = builder.Build();

app.UseWebSockets();

// Rota para WebSockets
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var notificationService = context.RequestServices.GetRequiredService<NotificationService>();

            // Tenta obter o userId do token JWT -
            // Se não encontrar userId, gera um identificador aleatório
            var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.NewGuid().ToString();

            await notificationService.HandleWebSocketConnection(userId, socket);
        }
        else context.Response.StatusCode = 400;

    }
    else await next();
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();