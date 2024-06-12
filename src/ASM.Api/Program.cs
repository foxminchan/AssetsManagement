using ASM.Api.Middleware;
using ASM.Application;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.IdentityAggregate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "NashTech ASM API", Version = "v1" }));

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddProblemDetails();

builder
    .AddInfrastructure()
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.MapEndpoints();

app
    .MapGroup("/api")
    .MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();
