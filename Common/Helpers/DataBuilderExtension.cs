using System;
using Microsoft.EntityFrameworkCore;
using TinyDemo.Models;

namespace TinyDemo.Common.Helpers
{
    public static class DataBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "admin@mail.com",
                    Password = "password",
                    RoleId = 1

                }
            );

        }
    }
}
