using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SupportTicketSystem.Data;

namespace SupportTicketSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // اتصال به دیتابیس
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Information()
            //.WriteTo.File(
            //    path: @"C:\MyAppLogs\app-.log",
            //    rollingInterval: RollingInterval.Day,
            //    retainedFileCountLimit: 30,
            //    shared: true)
            //.CreateLogger();

            //builder.Host.UseSerilog();
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddSignalR();
            // Razor Pages
            builder.Services.AddRazorPages();

            // Session
            builder.Services.AddSession();

            // احراز هویت با کوکی
            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Login";
                });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseSession();
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers(); 

            app.Run();
        }
    }
}
