using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;
using Microsoft.OpenApi.Models;
using PowerPlantAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseAllOfForInheritance();
    c.ExampleFilters();
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ProductionPlan",
        Description = "powerplant-coding-challenge",
        Contact = new OpenApiContact
        {
            Name   = "Hofbauer Benoit", 
            Email = "benoit@bh-it.be"
        }
    });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();


builder.Services.AddScoped<IProductionPlanService,ProductionPlanService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
