using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PolePosition.API.Common;
using PolePosition.API.Models;

namespace PolePosition.API.Data;

public static class DbSeeder
{
    // Documented in README.md as test credentials
    private const string TestPassword = "Test1234";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync();

        await EnsureUserAsync(context, userManager, "desktop@poleposition.ba", "Desktop", "Admin", Roles.Admin);
        await EnsureUserAsync(context, userManager, "mobile@poleposition.ba", "Mobile", "Vozac", Roles.Driver);
        await EnsureUserAsync(context, userManager, "instruktor@poleposition.ba", "Instruktor", "Test", Roles.Instructor);
    }

    private static async Task EnsureUserAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        string email,
        string firstName,
        string lastName,
        string role)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FirstName = firstName,
            LastName = lastName
        };

        // UserManager saves twice (user, then role), so both steps share one transaction
        await using var transaction = await context.Database.BeginTransactionAsync();

        var createResult = await userManager.CreateAsync(user, TestPassword);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Seeding user {email} failed: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        await userManager.AddToRoleAsync(user, role);
        await transaction.CommitAsync();
    }
}