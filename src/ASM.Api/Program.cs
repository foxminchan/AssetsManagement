using ASM.Api;
using ASM.Api.Middleware;
using ASM.Application;
using ASM.Application.Common.Converter;
using ASM.Application.Common.Endpoints;
using ASM.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Any;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NashTech ASM API", Version = "v1" });
    c.MapType<DateOnly>(() => new()
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
    });
    c.CustomSchemaIds(type => type.ToString());
});

builder.Services.Configure<JsonOptions>(
    options => options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter()));

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddInfrastructure().AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

app.UseSwagger();
app.UseReDoc();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseExceptionHandler();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}
else
{
    app.UseHsts();
    app.UseExceptionHandler("/error");
    app.MapGet("/", () => Results.Redirect("/api-docs")).ExcludeFromDescription();
}

await app.InitializeDatabaseAsync();

app.UseHttpsRedirection();
app.MapEndpoints();
app.MapIdentityEndpoints();
app.UseCors("AllowAll");
app.UseAuthorization();

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
