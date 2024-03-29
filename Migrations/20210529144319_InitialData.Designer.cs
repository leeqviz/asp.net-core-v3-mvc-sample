﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Models.Database;

namespace WebApp.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210529144319_InitialData")]
    partial class InitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApp.Models.Database.Entities.Administrator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UniqueKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Administrators");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            UniqueKey = "adm-1",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LiteratureId")
                        .HasColumnType("int");

                    b.Property<int?>("PublicationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LiteratureId");

                    b.HasIndex("PublicationId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("CopyId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CopyId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("FIO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniqueKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            UniqueKey = "cln-1",
                            UserId = 3
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "test",
                            Description = "test",
                            ImagePath = "",
                            Name = "test company",
                            PhoneNumber = "123456789",
                            Type = "library"
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Copy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<int?>("PublicationId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StorageId")
                        .HasColumnType("int");

                    b.Property<int?>("Time")
                        .HasColumnType("int");

                    b.Property<string>("UniqueKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PublicationId");

                    b.HasIndex("StorageId");

                    b.ToTable("Copies");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("StorageId")
                        .HasColumnType("int");

                    b.Property<string>("UniqueKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StorageId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StorageId = 1,
                            UniqueKey = "eml-1",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Filter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GenreId")
                        .HasColumnType("int");

                    b.Property<int?>("PublicationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("PublicationId");

                    b.ToTable("Filters");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PublicationId")
                        .HasColumnType("int");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique()
                        .HasFilter("[ClientId] IS NOT NULL");

                    b.HasIndex("PublicationId")
                        .IsUnique()
                        .HasFilter("[PublicationId] IS NOT NULL");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Literature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Literatures");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("CopyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CopyId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FIO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OperationId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OperationId")
                        .IsUnique()
                        .HasFilter("[OperationId] IS NOT NULL");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PublisherId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("Publications");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Publishers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "test publication"
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "employee"
                        },
                        new
                        {
                            Id = 3,
                            Name = "client"
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Storage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Storages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CompanyId = 1,
                            Name = "test storage"
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@test.com",
                            ImagePath = "~/src/img/profiles/default.png",
                            Name = "admin",
                            Password = "admin",
                            RegistrationDate = new DateTime(2021, 5, 29, 17, 43, 18, 504, DateTimeKind.Local).AddTicks(1682),
                            RoleId = 1
                        },
                        new
                        {
                            Id = 2,
                            Email = "employee@test.com",
                            ImagePath = "~/src/img/profiles/default.png",
                            Name = "employee",
                            Password = "employee",
                            RegistrationDate = new DateTime(2021, 5, 29, 17, 43, 18, 505, DateTimeKind.Local).AddTicks(3032),
                            RoleId = 2
                        },
                        new
                        {
                            Id = 3,
                            Email = "client@test.com",
                            ImagePath = "~/src/img/profiles/default.png",
                            Name = "client",
                            Password = "client",
                            RegistrationDate = new DateTime(2021, 5, 29, 17, 43, 18, 505, DateTimeKind.Local).AddTicks(3085),
                            RoleId = 3
                        });
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Administrator", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.User", "User")
                        .WithOne("Administrator")
                        .HasForeignKey("WebApp.Models.Database.Entities.Administrator", "UserId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Book", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Literature", "Literature")
                        .WithMany("Books")
                        .HasForeignKey("LiteratureId");

                    b.HasOne("WebApp.Models.Database.Entities.Publication", "Publication")
                        .WithMany("Books")
                        .HasForeignKey("PublicationId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Cart", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Client", "Client")
                        .WithMany("Carts")
                        .HasForeignKey("ClientId");

                    b.HasOne("WebApp.Models.Database.Entities.Copy", "Copy")
                        .WithMany("Carts")
                        .HasForeignKey("CopyId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Client", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Category", "Category")
                        .WithMany("Clients")
                        .HasForeignKey("CategoryId");

                    b.HasOne("WebApp.Models.Database.Entities.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("WebApp.Models.Database.Entities.Client", "UserId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Copy", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Publication", "Publication")
                        .WithMany("Copies")
                        .HasForeignKey("PublicationId");

                    b.HasOne("WebApp.Models.Database.Entities.Storage", "Storage")
                        .WithMany("Copies")
                        .HasForeignKey("StorageId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Employee", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Storage", "Storage")
                        .WithMany("Employees")
                        .HasForeignKey("StorageId");

                    b.HasOne("WebApp.Models.Database.Entities.User", "User")
                        .WithOne("Employee")
                        .HasForeignKey("WebApp.Models.Database.Entities.Employee", "UserId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Filter", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Genre", "Genre")
                        .WithMany("Filters")
                        .HasForeignKey("GenreId");

                    b.HasOne("WebApp.Models.Database.Entities.Publication", "Publication")
                        .WithMany("Filters")
                        .HasForeignKey("PublicationId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.History", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Client", "Client")
                        .WithOne("History")
                        .HasForeignKey("WebApp.Models.Database.Entities.History", "ClientId");

                    b.HasOne("WebApp.Models.Database.Entities.Publication", "Publication")
                        .WithOne("History")
                        .HasForeignKey("WebApp.Models.Database.Entities.History", "PublicationId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Literature", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Author", "Author")
                        .WithMany("Literatures")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Operation", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Client", "CLient")
                        .WithMany("Operations")
                        .HasForeignKey("ClientId");

                    b.HasOne("WebApp.Models.Database.Entities.Copy", "Copy")
                        .WithMany("Operations")
                        .HasForeignKey("CopyId");

                    b.HasOne("WebApp.Models.Database.Entities.Employee", "Employee")
                        .WithMany("Operations")
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Order", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Operation", "Operation")
                        .WithOne("Order")
                        .HasForeignKey("WebApp.Models.Database.Entities.Order", "OperationId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Publication", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Publisher", "Publisher")
                        .WithMany("Publications")
                        .HasForeignKey("PublisherId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.Storage", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Company", "Company")
                        .WithMany("Storages")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("WebApp.Models.Database.Entities.User", b =>
                {
                    b.HasOne("WebApp.Models.Database.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
