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
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionStringDB"); // Make sure this matches your appsettings.json
            builder.Services.AddDbContext<SwpFall24Context>(options =>
                options.UseSqlServer(connectionString)); // Ensure the connection string and SQL provider are correct

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

            // Correct the route to match your controller methods
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Mentor}/{action=SetSchedule}/{id?}"); // Set the correct action name

            app.Run();
        }
    }
}
