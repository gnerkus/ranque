using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Presentation;
using Presentation.ActionFilters;
using Sentry.Extensibility;
using Serilog;
using Service.DataShaping;
using Shared;
using streak;
using streak.Extensions;
using streak.Utility;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfig) => 
    loggerConfig
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
        )
        .ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.WebHost.UseSentry(options =>
{
    options.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");
    options.TracesSampleRate = 1.0;
    options.SendDefaultPii = true;
    options.MinimumBreadcrumbLevel = LogLevel.Debug;
    options.MinimumEventLevel = LogLevel.Warning;
    options.Debug = true;
    options.DiagnosticLevel = SentryLevel.Error;
    options.MaxRequestBodySize = RequestSize.Always;
});

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IDataShaper<ParticipantDto>, DataShaper<ParticipantDto>>();
builder.Services.AddScoped<IDataShaper<ScoreDto>, DataShaper<ScoreDto>>();
builder.Services.AddScoped<IDataShaper<LeaderboardDto>, DataShaper<LeaderboardDto>>();
builder.Services.AddScoped<IParticipantLinks, ParticipantLinks>();
builder.Services.AddScoped<IScoreLinks, ScoreLinks>();
builder.Services.AddScoped<ILeaderboardLinks, LeaderboardLinks>();

// Add services to the container.
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.InputFormatters.Insert(
            0,
            new ServiceCollection()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider()
                .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First()
        );
    })
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCSVFormatter()
    .AddApplicationPart(typeof(IAssemblyReference).Assembly);
builder.Services.AddProblemDetails();

builder.Services.AddCustomMediaTypes();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureOutputCaching();
builder.Services.ConfigureRateLimitingOptions();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(opt => { });
// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
    app.UseHsts();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRateLimiter();
app.UseCors("CorsPolicy");
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public partial class Program {}