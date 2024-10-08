using MentorScheduleCustom.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorScheduleCustom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add the DbContext for Entity Framework Core, using a connection string from the configuration (appsettings.json)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionStringDB");
            builder.Services.AddDbContext<SwpFall24Context>(options =>
                options.UseSqlServer(connectionString)); // Make sure to adjust the connection string name and database provider

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Mentor}/{action=ScheduleByWeek}/{id?}");

            app.Run();
        }
    }
}
