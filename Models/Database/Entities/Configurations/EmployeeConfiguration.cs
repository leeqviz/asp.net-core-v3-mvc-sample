using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(new Employee[]
            {
                new Employee { Id = 1, UserId = 2, UniqueKey = "eml-1", StorageId = 1 },
                new Employee { Id = 2, UserId = 3, UniqueKey = "eml-2", StorageId = 2 },
                new Employee { Id = 3, UserId = 4, UniqueKey = "eml-3", StorageId = 3 },
                new Employee { Id = 4, UserId = 5, UniqueKey = "eml-4", StorageId = 4 },
                new Employee { Id = 5, UserId = 6, UniqueKey = "eml-5", StorageId = 5 },
                new Employee { Id = 6, UserId = 7, UniqueKey = "eml-6", StorageId = 6 }
            });
        }
    }
}
