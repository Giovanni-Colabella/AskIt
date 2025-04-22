using AskIt.Models.Authorization;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using AskIt.Models.Services.Application.Account;
using AskIt.Models.Services.Application.QuestionService;
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
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(Policies.CanDeleteQuestion), policy =>
    {
        policy.Requirements.Add(new DeleteQuestionRequirement());
    });
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, EfCoreAuthService>();
builder.Services.AddScoped<IQuestionService, EfCoreQuestionService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDataInitializer.SeedRoles(services);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
