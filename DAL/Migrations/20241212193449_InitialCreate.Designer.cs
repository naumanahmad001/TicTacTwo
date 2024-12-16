﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241212193449_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Domain.Config", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardSize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConfigName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("GridSize")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PiecesAmount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerOnePiece")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerTwoPiece")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConfigId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstPlayerPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GridTopLeft")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PositionsJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecondPlayerPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.ToTable("SavedGames");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.HasOne("Domain.Config", "Config")
                        .WithMany("Games")
                        .HasForeignKey("ConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Config");
                });

            modelBuilder.Entity("Domain.Config", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
