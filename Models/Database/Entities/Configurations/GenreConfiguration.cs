using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(new Genre[]
            {
                new Genre { Id = 1, Name = "фэнтези" },
                new Genre { Id = 2, Name = "фантастика" },
                new Genre { Id = 3, Name = "ужасы" },
                new Genre { Id = 4, Name = "художественная литература" },
                new Genre { Id = 5, Name = "роман-антиутопия" },
            });
        }
    }
}
