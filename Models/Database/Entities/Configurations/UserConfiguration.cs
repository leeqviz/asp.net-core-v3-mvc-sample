using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User[]
            {
                new User { Id = 1, RoleId = 1, Name = "admin", Email = "admin@test.com", Password = "admin", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 2, RoleId = 2, Name = "employee1", Email = "employee1@test.com", Password = "employee", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 3, RoleId = 2, Name = "employee2", Email = "employee2@test.com", Password = "employee", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 4, RoleId = 2, Name = "employee3", Email = "employee3@test.com", Password = "employee", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 5, RoleId = 2, Name = "employee4", Email = "employee4@test.com", Password = "employee", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 6, RoleId = 2, Name = "employee5", Email = "employee5@test.com", Password = "employee", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 7, RoleId = 2, Name = "employee6", Email = "employee6@test.com", Password = "employee", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now },
                new User { Id = 8, RoleId = 3, Name = "client", Email = "client@test.com", Password = "client", ImagePath = "~/src/img/profiles/default.png", RegistrationDate = DateTime.Now }
            });
        }
    }
}
