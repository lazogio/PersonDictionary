using System.Resources;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonDictionary.Application;
using PersonDictionary.Application.Interface;
using PersonDictionary.Application.Mapping;
using PersonDictionary.Application.Models;
using PersonDictionary.Application.PipelineBehaviour;
using PersonDictionary.Application.ResourceManagerService;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.Interface;
using PersonDictionary.Middlewares;
using PersonDictionary.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var applicationAssembly = typeof(AssemblyReference).Assembly;

var connectionString = builder.Configuration.GetConnectionString("PersonDictionaryDB");
builder.Services.AddControllers(options => { options.Filters.Add(typeof(ValidationActionFilter)); })
    .AddApplicationPart(applicationAssembly)
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Person Dictionary", Version = "v1" });
    c.OperationFilter<AcceptLanguageMiddleware.CustomHeaderSwaggerAttribute>();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(typeof(AssemblyReference));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddScoped<ValidationActionFilter>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IDtoToEntityMapper, DtoToEntityMapper>();
builder.Services.AddScoped<IMapper<Person, PersonModel>, PersonMapper>();
builder.Services.AddScoped<IMapper<Person, PersonDetailedModel>, PersonDetailedMapper>();
builder.Services.AddScoped<IMapper<PersonRelation, RelatedPersonsModel>, PersonRelationMapper>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IResourceManagerService>(_ =>
{
    var assembly = applicationAssembly;
    var resourceManager = new ResourceManager("PersonDictionary.Application.Resources.SharedResource", assembly);
    return new ResourceManagerService(resourceManager);
});
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
var app = builder.Build();

var dbInitializer = new DbInitializer();
await dbInitializer.Seed(app.Services, CancellationToken.None);

app.UseMiddleware<AcceptLanguageMiddleware>();
app.UseMiddleware<ErrorLoggingMiddleware>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();

var logFilePath = configuration["Logging:File:Path"] ?? "logs/app.txt";
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File(logFilePath)
    .CreateLogger();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();