using Lnk.DataAccess.Configuration;
using Lnk.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.AddDependencyInjection();
builder.Services.AddAutoMapper();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.AutoMigration();
app.SeedData().GetAwaiter().GetResult();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
