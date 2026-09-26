using Domain.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository.Data;
using Repository.Implementation;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


// ============================================================================================
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IDeploymentService, DeploymentService>();
builder.Services.AddScoped<IEmergencyServiceService, EmergencyServiceService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IResponseTeamService, ResponseTeamService>();
builder.Services.AddScoped<IStatusUpdateService, StatusUpdateService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddScoped<IPriorityCalculationService, PriorityCalculationService>();

builder.Services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.AddScoped<IWeatherSnapshotService, WeatherSnapshotService>();
builder.Services.AddHttpClient<IWeatherSnapshotApiClient, WeatherSnapshotApiClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<WeatherApiSettings>>();

    client.BaseAddress = new Uri(settings.Value.BaseAddress);
    client.Timeout = TimeSpan.FromSeconds(settings.Value.TimeoutSeconds);
});
// ============================================================================================


var app = builder.Build();


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

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();