using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;

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
            builder.Services.AddSignalR();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<INotificationService, NotificationService>();
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
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandler =
                        context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

                    if (exceptionHandler?.Error is Exception ex)
                    {
                        Log.Error(ex, "خطا در شروع برنامه");
                    }

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "خطا در شروع برنامه."
                    });
                });
            });
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
            app.MapHub<NotificationHub>("/hubs/notifications");

            app.Run();
        }
    }
}
