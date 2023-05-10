﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinimalApiLabb3.Data;

#nullable disable

namespace MinimalApiLabb3.Migrations
{
    [DbContext(typeof(Labb3MinmalContext))]
    [Migration("20230510225831_addDefaultFKinterestIdLinkTable")]
    partial class addDefaultFKinterestIdLinkTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MinimalApiLabb3.Program+Interest", b =>
                {
                    b.Property<int>("InterestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InterestId"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("FK_PersonId")
                        .HasColumnType("int");

                    b.Property<string>("InterestDescription")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("InterestTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("InterestId");

                    b.HasIndex("FK_PersonId");

                    b.ToTable("Interests");
                });

            modelBuilder.Entity("MinimalApiLabb3.Program+Link", b =>
                {
                    b.Property<int>("LinkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LinkId"));

                    b.Property<int?>("FK_InterestId")
                        .HasColumnType("int");

                    b.Property<string>("LinkTitle")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LinkId");

                    b.HasIndex("FK_InterestId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("MinimalApiLabb3.Program+Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("MinimalApiLabb3.Program+Interest", b =>
                {
                    b.HasOne("MinimalApiLabb3.Program+Person", "Persons")
                        .WithMany("Interests")
                        .HasForeignKey("FK_PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persons");
                });

            modelBuilder.Entity("MinimalApiLabb3.Program+Link", b =>
                {
                    b.HasOne("MinimalApiLabb3.Program+Interest", "Interest")
                        .WithMany("Links")
                        .HasForeignKey("FK_InterestId");

                    b.Navigation("Interest");
                });

            modelBuilder.Entity("MinimalApiLabb3.Program+Interest", b =>
                {
                    b.Navigation("Links");
                });

            modelBuilder.Entity("MinimalApiLabb3.Program+Person", b =>
                {
                    b.Navigation("Interests");
                });
#pragma warning restore 612, 618
        }
    }
}
