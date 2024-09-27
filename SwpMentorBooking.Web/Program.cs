using Microsoft.EntityFrameworkCore;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Infrastructure.Configuration;
using SwpMentorBooking.Infrastructure.Data;
using SwpMentorBooking.Infrastructure.Repository;
using SwpMentorBooking.Infrastructure.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add DbContext

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register UnitOfWork & FileService

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CSVFileService>();
builder.Services.AddScoped<AutoMapperService>();
builder.Services.AddScoped<PasswordGeneratorService>();

// Add Auto-mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// Configure Kestrel
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 52428800; // 50 MB
});


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
    pattern: "{controller=Administrator}/{action=Index}/{id?}");

app.Run();
