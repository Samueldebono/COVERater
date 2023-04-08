﻿// <auto-generated />
using System;
using COVERater.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COVERater.API.Migrations
{
    [DbContext(typeof(CoveraterContext))]
    [Migration("20221014125750_add logs table")]
    partial class addlogstable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("COVERater.API.Models.AuthUsers", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("ExperienceLevel")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("HashUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("RoleType")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("AuthUsers");
                });

            modelBuilder.Entity("COVERater.API.Models.EmailLogs", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailSent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("EmailLogs");
                });

            modelBuilder.Entity("COVERater.API.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CloudinaryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Delete")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ImageId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("COVERater.API.Models.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("After")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Before")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Function")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("COVERater.API.Models.SubImage", b =>
                {
                    b.Property<int>("SubImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CloudinaryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("CoverageRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Delete")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("SubImage");
                });

            modelBuilder.Entity("COVERater.API.Models.Token", b =>
                {
                    b.Property<int>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserGuid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TokenId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("COVERater.API.Models.UserEmails", b =>
                {
                    b.Property<int>("UserEmailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PrizeSent")
                        .HasColumnType("bit");

                    b.HasKey("UserEmailId");

                    b.ToTable("UserEmails");
                });

            modelBuilder.Entity("COVERater.API.Models.UserStats", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthUsersRoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FinishedPhaseUtc")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("FinishingPercentPhase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<byte>("Phase")
                        .HasColumnType("tinyint");

                    b.Property<int?>("PictureCycledPhase")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TimePhase")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.HasIndex("AuthUsersRoleId");

                    b.ToTable("UserStats");
                });

            modelBuilder.Entity("COVERater.API.Models.UsersGuess", b =>
                {
                    b.Property<int>("UsersGuessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("GuessPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("GuessTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Phase")
                        .HasColumnType("tinyint");

                    b.Property<int>("SubImageId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UsersGuessId");

                    b.HasIndex("SubImageId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersGuess");
                });

            modelBuilder.Entity("COVERater.API.Models.SubImage", b =>
                {
                    b.HasOne("COVERater.API.Models.Image", "Image")
                        .WithMany("SubImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("COVERater.API.Models.UserStats", b =>
                {
                    b.HasOne("COVERater.API.Models.AuthUsers", null)
                        .WithMany("UserStats")
                        .HasForeignKey("AuthUsersRoleId");
                });

            modelBuilder.Entity("COVERater.API.Models.UsersGuess", b =>
                {
                    b.HasOne("COVERater.API.Models.SubImage", "SubImage")
                        .WithMany()
                        .HasForeignKey("SubImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("COVERater.API.Models.UserStats", "UserStats")
                        .WithMany("Guesses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
