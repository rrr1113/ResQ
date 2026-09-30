using System;
using System.Threading.Channels;
using System.Threading.RateLimiting;
using Domain.Configuration;
using Domain.Dto.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository.Data;
using Repository.Implementation;
using Repository.Interface;
using Service;
using Service.Implementation;
using Service.Implementation.Excel;
using Service.Interface;
using Service.Interface.Excel;
using Web.Interceptor;
using Web.Mappers;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<AuditInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseSqlite(connectionString)
        .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================================================================
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IDeploymentService, DeploymentService>();
builder.Services.AddScoped<IEmergencyServiceService, EmergencyServiceService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IResponseTeamService, ResponseTeamService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITeamAssigmentService, TeamAssignmentService>();
builder.Services.AddScoped<IPriorityCalculatorService, PriorityCalculatorService>();

builder.Services.AddScoped<PriorityCalculatorService, PriorityCalculatorService>();

builder.Services.Configure<GeocodeApiSettings>(builder.Configuration.GetSection("GeocodeApi"));
builder.Services.AddHttpClient<IGeocodingApiClient, EtlGeocodingApiClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<GeocodeApiSettings>>();

    client.BaseAddress = new Uri(settings.Value.BaseAddress);
    client.Timeout = TimeSpan.FromSeconds(settings.Value.TimeoutSeconds);
    
    client.DefaultRequestHeaders.UserAgent.ParseAdd("ResQ-EmergencyPlatform/1.0 (" + settings.Value.UserEmail +")");
});

builder.Services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.AddScoped<IWeatherSnapshotService, WeatherSnapshotService>();
builder.Services.AddHttpClient<IWeatherSnapshotApiClient, WeatherSnapshotApiClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<WeatherApiSettings>>();

    client.BaseAddress = new Uri(settings.Value.BaseAddress);
    client.Timeout = TimeSpan.FromSeconds(settings.Value.TimeoutSeconds);
});

builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
builder.Services.AddSingleton(Channel.CreateUnbounded<EmailMessage>());builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHostedService<EmailBackgroundService>();

builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

builder.Services.Configure<ApiKeySettings>(builder.Configuration.GetSection("ApiKeySettings"));

builder.Services.Configure<RateLimitSettings>(builder.Configuration.GetSection("RateLimitSettings"));

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    options.AddPolicy("external-api", context =>
    {
        var settings = context.RequestServices
            .GetRequiredService<IOptions<RateLimitSettings>>().Value;
        var apiKey = context.Request.Headers["X-Api-Key"].ToString();

        return RateLimitPartition.GetFixedWindowLimiter(apiKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = settings.PermitLimit,
            Window = TimeSpan.FromSeconds(settings.WindowInSeconds),
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });
});


builder.Services.AddScoped<IncidentMapper>();
builder.Services.AddScoped<DeploymentMapper>();
builder.Services.AddScoped<ResponseTeamMapper>();
builder.Services.AddScoped<EmergencyServiceMapper>();
builder.Services.AddScoped<LocationMapper>();
builder.Services.AddScoped<VehicleMapper>();


// ============================================================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseRateLimiter();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();