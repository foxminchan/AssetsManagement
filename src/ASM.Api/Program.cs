using ASM.Api;
using ASM.Api.Middleware;
using ASM.Application;
using ASM.Application.Common.Endpoints;
using ASM.Application.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NashTech ASM API", Version = "v1" });
});

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddInfrastructure().AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder
            .WithOrigins(appSettings.CorsSettings.AllowedOrigins)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler();
    await app.InitializeDatabaseAsync();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.MapEndpoints();

app.MapIdentityEndpoints();

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

await app.RunAsync();
