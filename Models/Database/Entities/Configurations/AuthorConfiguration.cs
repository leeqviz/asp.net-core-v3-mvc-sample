using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData(new Author[]
            {
                new Author { Id = 1, Name = "Джон Рональд Руэл Толкин", ImagePath = "~/src/img/authors/1.jpg", Description = "" },
                new Author { Id = 2, Name = "Джоан Кэтлин Роулинг", ImagePath = "~/src/img/authors/2.jpg", Description = "" },
                new Author { Id = 3, Name = "Джордж Оруэлл", ImagePath = "~/src/img/authors/3.jpg", Description = "" },
                new Author { Id = 4, Name = "Олдос Хаксли", ImagePath = "~/src/img/authors/4.jpg", Description = "" },
                new Author { Id = 5, Name = "Уильям Джеральд Голдинг", ImagePath = "~/src/img/authors/5.jpg", Description = "" },
                new Author { Id = 6, Name = "Роберт Ирвин Говард", ImagePath = "~/src/img/authors/6.jpg", Description = "" },
                new Author { Id = 7, Name = "Говард Филлипс Лавкрафт", ImagePath = "~/src/img/authors/7.jpg", Description = "" },
                new Author { Id = 8, Name = "Франц Кафка", ImagePath = "~/src/img/authors/8.jpg", Description = "" },
                new Author { Id = 9, Name = "Стивен Эдвин Кинг", ImagePath = "~/src/img/authors/9.jpg", Description = "" },
                new Author { Id = 10, Name = "Фрэнк Патрик Герберт", ImagePath = "~/src/img/authors/10.jpg", Description = "" },
                new Author { Id = 11, Name = "Глен Чарльз Кук", ImagePath = "~/src/img/authors/11.jpg", Description = "" }
            });
        }
    }
}
