using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDo_App.Data.Context;
using ToDo_App.Hubs;
using ToDo_App.Services;
using ToDo_App.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Kestrel Configuration
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5076); // HTTP
    options.ListenAnyIP(7167, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
#endregion

#region Services

// Controllers + JSON Enum Support
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Database
builder.Services.AddDbContext<ToDoAppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ToDoWebAppDbConnection")
    );
});

// Dependency Injection
builder.Services.AddScoped<IToDoAppServices, ToDoAppServices>();
builder.Services.AddScoped<IAuthService, AuthService>();

// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

// CORS (restrict in production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // change for production
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new Exception("JWT Key is missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/todoHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Authorization (example policy)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Swagger + JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

#endregion

var app = builder.Build();

#region Middleware Pipeline

// Static Files (for frontend)
app.UseDefaultFiles();
app.UseStaticFiles();

// Compression
app.UseResponseCompression();

// Swagger (dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowFrontend");

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();
app.MapHub<TodoHub>("/todoHub");

#endregion

app.Run();