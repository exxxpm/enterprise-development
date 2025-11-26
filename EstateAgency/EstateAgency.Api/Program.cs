using EstateAgency.Application;
using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Interfaces;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Application.Services;
using EstateAgency.Domain;
using EstateAgency.Domain.Data;
using EstateAgency.Domain.Entitites;
using EstateAgency.Infrastructrure.EfCore.Persistence;
using EstateAgency.Infrastructrure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Estate Agency API",
        Version = "v1",
        Description = "API Documentation"
    });
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (var xmlFile in xmlFiles)
    {
        c.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
    }
});

builder.AddSqlServerDbContext<EstateAgencyDbContext>("DefaultConnection");

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<EstateAgencyMappingProfile>());

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ICrudService<ApplicationGetDto, ApplicationCreateEditDto>, ApplicationService>();
builder.Services.AddScoped<ICrudService<PropertyGetDto, PropertyCreateEditDto>, PropertyService>();
builder.Services.AddScoped<ICrudService<CounterpartyGetDto, CounterpartyCreateEditDto>, CounterpartyService>();

builder.Services.AddScoped<AnalyticService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EstateAgencyDbContext>();
    dbContext.Database.Migrate();
    DbSeeder.Seed(dbContext, new DataSeeder());
}

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estate Agency v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
