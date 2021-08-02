using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new Role[]
            {
                new Role { Id = 1, Name = "admin" },
                new Role { Id = 2, Name = "employee" },
                new Role { Id = 3, Name = "client" }
            });
        }
    }
}
