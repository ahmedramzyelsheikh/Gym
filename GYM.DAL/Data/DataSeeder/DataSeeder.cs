using System.Text.Json;
using GYM.DAL.Data.Enums;
using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GYM.DAL.Data.DataSeeder
{
    public static class DataSeeder
    {
        // Applies pending migrations, then seeds categories and plans (plans come from plans.json).
        public static async Task SeedAsync(GymDbContext.GYMDbContext context, string seedFilesPath, ILogger logger)
        {
            try
            {
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();
                }

                await SeedCategoriesAsync(context);
                await SeedPlansAsync(context, Path.Combine(seedFilesPath, "plans.json"), logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedCategoriesAsync(GymDbContext.GYMDbContext context)
        {
            var existing = await context.Categories.Select(c => c.CategoryName).ToListAsync();
            var missing = Enum.GetNames<CategoryNames>()
                              .Where(name => !existing.Contains(name))
                              .Select(name => new Category { CategoryName = name })
                              .ToList();

            if (missing.Count == 0) return;

            context.Categories.AddRange(missing);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPlansAsync(GymDbContext.GYMDbContext context, string plansFile, ILogger logger)
        {
            if (!File.Exists(plansFile))
            {
                logger.LogWarning("Plans seed file not found: {File}", plansFile);
                return;
            }

            await using var stream = File.OpenRead(plansFile);
            var plans = await JsonSerializer.DeserializeAsync<List<Plan>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Plan>();

            var existing = await context.Plans.Select(p => p.Name).ToListAsync();
            var missing = plans.Where(p => !existing.Contains(p.Name)).ToList();

            if (missing.Count == 0) return;

            context.Plans.AddRange(missing);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} plans.", missing.Count);
        }
    }
}
