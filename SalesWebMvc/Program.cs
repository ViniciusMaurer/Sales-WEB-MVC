﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("SalesWebMvcContext"),
        new MySqlServerVersion(new Version(8, 0, 40)) 
    )
);

builder.Services.AddScoped<SeedingService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Executa o seed de dados apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var seedingService = services.GetRequiredService<SeedingService>();
        seedingService.Seed();
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
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();