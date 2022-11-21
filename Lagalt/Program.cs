using Lagalt.Connections;
using Lagalt.Hubs;
using Lagalt.Models;
using Lagalt.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Lagalt API",
        Description = "An ASP.NET Core Web API for Lagalt case project",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<LagaltContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<ProjectRepo>();
builder.Services.AddScoped<ApplicantRepo>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllHeaders",
        corsbuilder =>
        {
            corsbuilder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000", "https://theprojectlagalt.web.app", "https://www.lagaltapi.somee.com");
        });
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://theprojectlagalt.web.app", "https://www.lagaltapi.somee.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
builder.Services.AddSignalR();

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();
