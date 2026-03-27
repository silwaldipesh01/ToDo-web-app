using System.Text.Json.Serialization;
using ToDo_App.Data.Context;
using Microsoft.EntityFrameworkCore;
using ToDo_App.Services.Interfaces;
using ToDo_App.Services;


var builder = WebApplication.CreateBuilder(args);

// 1. Keep your CORS settings
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddDbContext<ToDoAppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ToDoWebAppDbConnection")
    );
});
builder.Services.AddTransient<IToDoAppServices, ToDoAppServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. ENABLE HTML SUPPORT
app.UseDefaultFiles(); // This looks for index.html automatically
app.UseStaticFiles();  // This allows serving .html, .js, and .css files

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();