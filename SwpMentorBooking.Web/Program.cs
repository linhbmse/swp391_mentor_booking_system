using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddScoped<ICSVFileService, CSVFileService>();
builder.Services.AddScoped<IAutoMapperService, AutoMapperService>();
builder.Services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
builder.Services.AddScoped<IMIMEFileService, MIMEFileService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStringManipulationService, StringManipulationService>();
builder.Services.AddScoped<IUtilService, UtilService>();
// Add Auto-mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// Add Authentication
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = $"/Account/Login";
        option.LogoutPath = $"/Account/Logout";
        option.AccessDeniedPath = $"/Account/AccessDenied";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    });
// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MentorPolicy", policy => policy.RequireRole("Mentor"));
    options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
});
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
