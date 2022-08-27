using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GuacosTracker3.Data;
using GuacosTracker3.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

//DI for DbContext
builder.Services.AddDbContext<TrackerDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDefaultIdentity<TrackerUser>(options => options.SignIn.RequireConfirmedAccount = false) //used to check confirmation email
    .AddEntityFrameworkStores<TrackerDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
