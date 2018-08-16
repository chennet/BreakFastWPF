using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Database;

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180816083707_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Database.BFMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Desp");

                    b.Property<int>("Discount");

                    b.Property<string>("ImageUri");

                    b.Property<string>("MenuId");

                    b.Property<string>("MenuType");

                    b.Property<int>("Price");

                    b.Property<string>("Style");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("BFMenus");
                });

            modelBuilder.Entity("Database.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });
        }
    }
}
