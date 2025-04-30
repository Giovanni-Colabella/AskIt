using AskIt.Models.Authorization;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using AskIt.Models.Services.Application.Account;
using AskIt.Models.Services.Application.CourseService;
using AskIt.Models.Services.Application.ForumService;
using AskIt.Models.Services.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Error/UnauthorizedError";
});

builder.Services.AddAuthorization(options =>
{
    // Registrazione varie policy 
});
// Registra Authorization Handlers
builder.Services.AddScoped<IAuthorizationHandler, DeleteAnswerHandler>(); // Scoped, dipende da un servizio dbContext
builder.Services.AddSingleton<IAuthorizationHandler, DeleteQuestionHandler>(); // Singleton, non dipende da un servizio dbContext
builder.Services.AddScoped<IAuthorizationHandler, EditCourseHandler>(); // Scoped, dipende da un servizio dbContext

// Servizi ASP.Net 
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// Servizi Applicativi e Infrastrutturali
builder.Services.AddScoped<IAuthService, EfCoreAuthService>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<ICachedForumService, MemoryCacheForumService>();
builder.Services.AddScoped<ICourseService, EfCoreCourseService>();
builder.Services.AddSingleton<IImagePersister, MagickNetImagePersister>(); // Importante: singleton, contiene un semaforo

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDataInitializer.SeedRoles(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
} else {
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "forum",
    pattern: "{controller=Forum}/{action=Index}/{id?}");

app.Run();
