using System.Text.Json;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager,
                                       RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

        if (users == null) return;

        var roles = new List<AppRole> {
                new AppRole { Name = Roles.Member.ToString() },
                new AppRole { Name = Roles.Admin.ToString() },
                new AppRole { Name = Roles.Moderator.ToString() }
            };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
        foreach (var user in users)
        {
            // using var hmac = new HMACSHA512();
            user.Photos.First().IsApproved = true;
            user.UserName = user.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@$$w0rd"));
            // user.PasswordSalt = hmac.Key;

            // context.Users.Add(user);
            await userManager.CreateAsync(user, Constant.DefaultPassword);
            await userManager.AddToRoleAsync(user, Roles.Member.ToString());
        }

        var admin = new AppUser { UserName = "admin", KnownAs = "Admin", Gender = Gender.Male.ToString() };

        await userManager.CreateAsync(admin, Constant.DefaultPassword);
        // await userManager.AddToRoleAsync(admin, "Admin");
        await userManager.AddToRolesAsync(admin, new[] { Roles.Admin.ToString(), Roles.Moderator.ToString() });


        // await context.SaveChangesAsync();
    }
}
