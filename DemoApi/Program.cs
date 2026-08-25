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

const string ReactClientPolicy = "ReactClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(ReactClientPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5174", "http://localhost:5173","http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();




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

app.UseCors(ReactClientPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
