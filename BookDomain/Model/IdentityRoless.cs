using BookDomain.Model;
using Microsoft.AspNetCore.Identity;

using System;
using System.Threading.Tasks;

namespace BookDomain.Model;

public static class IdentityRoless
{
    public static async Task SeedRolesAndAdminAsync(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
    {
        // 1. Створення ролей, якщо їх немає
        string[] roleNames = {  "Buyer", "Admin" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        
        string adminEmail = "name@rr.com";
        string adminPassword = "6GYFjYeec+McW.Z";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdmin = new Customer
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdmin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}