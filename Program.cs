using Microsoft.EntityFrameworkCore;
using WebAdminScheduler.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Oracle.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
    Console.WriteLine("intentamos recuperar la configuracion "+(builder.Configuration.GetConnectionString("DefaultConnection") ?? ""));

builder.Services.AddDbContext<WebAdminSchedulerContext>(options => 
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection") ?? "")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calendario}/{action=Index}/{id?}");
app.Run();