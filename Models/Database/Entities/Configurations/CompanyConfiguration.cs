using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(new Company[]
            {
                new Company { Id = 1, Address = "ул. Калинина, 1", Type = "library", Name = "Библиотека им. В. Маяковского", Description = "Центральная библиотека", PhoneNumber = "123456789", ImagePath = "~/src/img/companies/lib1.jpg" },
                new Company { Id = 2, Address = "ул. Молодежная, 104", Type = "library", Name = "Библиотека им. Я. Коласа", Description = "Филиал №1", PhoneNumber = "123456789", ImagePath = "~/src/img/companies/lib2.jpg" },
                new Company { Id = 3, Address = "ул. Молодежная, 16", Type = "library", Name = "Библиотека им. К. Симонова", Description = "Филиал №2", PhoneNumber = "123456789", ImagePath = "~/src/img/companies/lib3.jpg" },
                new Company { Id = 4, Address = "ул. Молодежная, 106", Type = "library", Name = "Библиотека им. С. Маршака", Description = "Филиал №3", PhoneNumber = "123456789", ImagePath = "~/src/img/companies/lib4.jpg" },
                new Company { Id = 5, Address = "ул. Юбилейная, 1", Type = "library", Name = "Библиотека им. А. Пушкина", Description = "Филиал №4", PhoneNumber = "123456789", ImagePath = "~/src/img/companies/lib5.jpg" },
                new Company { Id = 6, Address = "ул. Молодежная, 169", Type = "library", Name = "Библиотека им. В. Короткевича", Description = "Филиал №5", PhoneNumber = "123456789", ImagePath = "~/src/img/companies/lib6.jpg" }
            });
        }
    }
}
