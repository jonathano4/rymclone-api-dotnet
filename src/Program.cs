using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using RymCloneApi.src.Exceptions.Handlers;
using RymCloneApi.src.Exceptions.InternalServerErrorException;
using RymCloneApi.src.Exceptions.NotFoundErrorException;
using RymCloneApi.src.Exceptions.UnprocessableEntityException;
using RymCloneApi.src.Persistence;
using RymCloneApi.src.Persistence.Context;
using RymCloneApi.src.Persistence.Context.Interfaces;
using RymCloneApi.src.Persistence.Repositories.Albums;
using RymCloneApi.src.Persistence.Repositories.Artists;
using RymCloneApi.src.Persistence.Repositories.Genres;
using RymCloneApi.src.Persistence.Repositories.Reviews;
using RymCloneApi.src.Persistence.UnitOfWork;
using RymCloneApi.src.Providers;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

DotEnv.AutoConfig();
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
  opt.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<JsonPatchExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<UnprocessableEntityExceptionHandler>();
builder.Services.AddExceptionHandler<InternalServerErrorExceptionHandler>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  options.JsonSerializerOptions.IncludeFields = true;
});
builder.Services.AddControllers().AddNewtonsoftJson();

//builder.Services.AddApplicationInsightsTelemetry(options =>
//{
//  options.ConnectionString = EnvProvider.Instance.GetStringValue("APP_INSIGHTS_TELEMETRY_CONNECTION_STRING");
//});

builder.Services.AddAuthentication()
  .AddGoogle(options =>
  {
    options.ClientId = EnvProvider.Instance.GetStringValue("GOOGLE_AUTH_CLIENT_ID");
    options.ClientSecret = EnvProvider.Instance.GetStringValue("GOOGLE_AUTH_CLIENT_SECRET");
  });

builder.Services.AddScoped<AppDbContextInitializer>();
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGenresRepository, GenresRepository>();
builder.Services.AddScoped<IAlbumsRepository, AlbumsRepository>();
builder.Services.AddScoped<IArtistsRepository, ArtistsRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();

var app = builder.Build();

app.UseExceptionHandler("/Error");

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference("/api-docs", options =>
  {
    options.WithTheme(ScalarTheme.Laserwave);
    options.ForceDarkMode();
  });
}

app.UseHttpsRedirection();

app.MapGet("/healthcheck", () => new { Message = "everything ok" }).WithDisplayName("Healthcheck");
app.MapControllers();

app.Run();
