using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class StorageConfigurations : IEntityTypeConfiguration<Storage>
    {
        public void Configure(EntityTypeBuilder<Storage> builder)
        {
            builder.HasData(new Storage[]
            {
                new Storage { Id = 1, Name = "хранилище 1", CompanyId = 1 },
                new Storage { Id = 2, Name = "хранилище 2", CompanyId = 2 },
                new Storage { Id = 3, Name = "хранилище 3", CompanyId = 3 },
                new Storage { Id = 4, Name = "хранилище 4", CompanyId = 4 },
                new Storage { Id = 5, Name = "хранилище 5", CompanyId = 5 },
                new Storage { Id = 6, Name = "хранилище 6", CompanyId = 6 }
            });
        }
    }
}
