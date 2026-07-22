using Basir.Domain.Entities.Identity;
using Basir.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Basir.Infrastructure.Identity;

public static class IdentitySeeder
{
    private static readonly SeedRole[] Roles =
    [
        new SeedRole("Admin", "Full system access with all administrative privileges."),
        new SeedRole("Moderator", "Content moderation and user management capabilities."),
        new SeedRole("User", "Standard application user with basic access.")
    ];

    private static readonly SeedUser[] Users =
    [
        new SeedUser("admin", "admin@basir.local", "System", "Administrator",
            "Admin@123456", UserStatus.Active, true, ["Admin"]),
        new SeedUser("moderator", "moderator@basir.local", "Content", "Moderator",
            "Moderator@123456", UserStatus.Active, true, ["Moderator", "User"]),
        new SeedUser("user", "user@basir.local", "Standard", "User",
            "User@123456", UserStatus.Active, true, ["User"])
    ];

    public static async Task SeedAsync(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        foreach (var seedRole in Roles)
        {
            if (!await roleManager.RoleExistsAsync(seedRole.Name))
            {
                var role = new ApplicationRole(seedRole.Name, seedRole.Description);
                await roleManager.CreateAsync(role);
            }
        }

        foreach (var seedUser in Users)
        {
            if (await userManager.FindByEmailAsync(seedUser.Email) is not null)
                continue;

            var user = new ApplicationUser
            {
                UserName = seedUser.UserName,
                NormalizedUserName = seedUser.UserName.ToUpperInvariant(),
                Email = seedUser.Email,
                NormalizedEmail = seedUser.Email.ToUpperInvariant(),
                FirstName = seedUser.FirstName,
                LastName = seedUser.LastName,
                EmailConfirmed = seedUser.EmailConfirmed,
                Status = seedUser.Status,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, seedUser.Password);
            if (result.Succeeded && seedUser.Roles.Length > 0)
            {
                await userManager.AddToRolesAsync(user, seedUser.Roles);
            }
        }
    }

    private sealed record SeedRole(string Name, string Description);

    private sealed record SeedUser(
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password,
        UserStatus Status,
        bool EmailConfirmed,
        string[] Roles);
}
