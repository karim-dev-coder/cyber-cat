using ApiGateway;
using ApiGateway.Extensions;
using ApiGateway.Repositories;
using ApiGateway.Services;
using ApiGateway.Services.BackgroundServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Используем строготипизированный класс, чтобы считать appsettings.json и удобно с ним работать.
builder.Services.Configure<ApiGatewayAppSettings>(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { options.TokenValidationParameters = JwtTokenValidation.CreateTokenParameters(); });

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(option => { option.AddJwtSecurityDefinition(builder.Configuration["Kestrel:Endpoints:Http:Url"]); });

builder.Services.AddScoped<IUserRepository, UserRepositoryMongoDb>();
builder.Services.AddHostedService<ApplyUserRepositoryMigrationsOnStart>();

builder.Services.AddScoped<IAuthUserService, AuthUserService>();
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepositoryMongoDb>();

builder.Services.AddScoped<ITaskRepository, TasksHierarchyRepository>();
builder.Services.AddScoped<ITaskRepository, TasksFlatRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ISolutionService, SolutionService>();
builder.Services.AddScoped<ISolutionRepository, SolutionRepository>();

var app = builder.Build();

// Если мы в режиме разработки (ASPNETCORE_ENVIRONMENT = Development).
if (app.Environment.IsDevelopment())
{
    // Показываем API спецификацию через swagger.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapFallback((context) =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        }
    );

    // Подробные ошибки в режиме разработки.
    app.UseDeveloperExceptionPage();
}

// Логирование Http запросов.
app.UseHttpLogging();

// Используем методы контроллеров как ендпоинты.
app.MapControllers();

app.Run();