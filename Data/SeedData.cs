using System.Linq;
using API.Entities;

namespace API.Data;

public static class SeedData
{
    public static void SeedDatabase(AppDbContext context)
    {
        // If Users DbSet is null, nothing to do
        if (context.Users == null) return;

        if (context.Users.Any()) return; // already seeded

        var users = new List<AppUser>
        {
            new AppUser { DisplayName = "Alice", Email = "alice@example.com" },
            new AppUser { DisplayName = "Bob", Email = "bob@example.com" },
            new AppUser { DisplayName = "Carol", Email = "carol@example.com" }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
