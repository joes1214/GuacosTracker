using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GuacosTracker3.Data;
using GuacosTracker3.Areas.Identity.Data;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using GuacosTracker3.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var configuration = builder.Configuration;

// Register IConfiguration in the DI container
builder.Services.AddSingleton(configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DI for DbContext
builder.Services.AddDbContext<TrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false) //used to check confirmation email
    .AddEntityFrameworkStores<TrackerDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

var app = builder.Build();

// Dynamically creates static paths
var viewPaths = Path.Combine(builder.Environment.ContentRootPath, "Views");
if (Directory.Exists(viewPaths))
{
    var viewFolders = Directory.GetDirectories(viewPaths);
    foreach(var viewFolder in viewFolders)
    {
        var staticFolder = Path.Combine(viewFolder, "static");
        if (Directory.Exists(staticFolder))
        {
            var viewName = new DirectoryInfo(viewFolder).Name;
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFolder),
                RequestPath = $"/{viewName}/Static"
            });
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();

app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LoginMiddleware>();

app.MapControllers();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Users}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
