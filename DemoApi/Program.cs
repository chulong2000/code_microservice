using DemoApi.Domain.IRepository;
using DemoApi.Domain.IServices;
using DemoApi.Infrastructure.Data;
using DemoApi.Infrastructure.Repository;
using DemoApi.Infrastructure.Service;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Data access: factory là Singleton (chỉ giữ chuỗi kết nối) ---
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(
    new Dictionary<string, string>
    {
        [DbConnectionNames.Default] = builder.Configuration.GetConnectionString("DemoConnection")!
    }));

// --- Session theo request: Scoped, để Repository/Service trong cùng request dùng chung ---
builder.Services.AddScoped<IDbSession, DbSession>();

// --- Repository / Service ---
builder.Services.AddScoped<IEducationLevelRepository, EducationLevelRepository>();
builder.Services.AddScoped<IEducationLevelService, EducationLevelService>();
builder.Services.AddScoped<IJobPositionRepository, JobPositionRepository>();
builder.Services.AddScoped<IJobPositionService,JobPositionService>();
builder.Services.AddScoped<IEducationLevelSalaryCoefficientRepository,EducationLevelSalaryCoefficientRepository>();
builder.Services.AddScoped<IEducationLevelSalaryCoefficientService, EducationLevelSalaryCoefficientService>();




// --- FluentValidation ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
