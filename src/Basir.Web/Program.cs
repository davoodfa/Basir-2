using Basir.Web.Extensions;
using Basir.Web.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddBasirServices(builder.Configuration);

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.MaxAge = TimeSpan.FromDays(365);
        options.IncludeSubDomains = true;
        options.Preload = true;
    });
}

var app = builder.Build();

await InitializeDatabaseAsync(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSecurityHeaders();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<Basir.Infrastructure.Identity.AppIdentityDbContext>();
    await db.Database.MigrateAsync();

    var roleManager = services
        .GetRequiredService<Microsoft.AspNetCore.Identity
            .RoleManager<Basir.Domain.Entities.Identity.ApplicationRole>>();
    var userManager = services
        .GetRequiredService<Microsoft.AspNetCore.Identity
            .UserManager<Basir.Domain.Entities.Identity.ApplicationUser>>();

    await Basir.Infrastructure.Identity.IdentitySeeder
        .SeedAsync(roleManager, userManager);
}
