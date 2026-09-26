using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

// ============================================================================================
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//TODO
builder.Services.AddScoped<IDeploymentService, DeploymentService>();
builder.Services.AddScoped<IEmergencyServiceService, EmergencyServiceService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IResponseTeamService, ResponseTeamService>();
builder.Services.AddScoped<IStatusUpdateService, StatusUpdateService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();


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