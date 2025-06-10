using BusinessLayer.Toolkit;
using RIACustomers.Database.Context;
using RIACustomers.Database.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(
        Options =>
            Options.JsonSerializerOptions.PropertyNamingPolicy = null
    )
;

builder.Services
    .RegisterPersistenceServices()
    .RegisterToolkit()
;

var app = builder.Build();

using var Scope = app.Services.CreateScope();

var Context = Scope.ServiceProvider.GetRequiredService<RIAContext>();
Context.Database.EnsureDeleted();
Context.Database.EnsureCreated();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
