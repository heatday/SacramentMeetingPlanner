﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SacramentMeetingPlanner.Data;

#nullable disable

namespace SacramentMeetingPlanner.Migrations
{
    [DbContext(typeof(SacramentMeetingPlannerContext))]
    [Migration("20230708032050_1")]
    partial class _1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SacramentMeetingPlanner.Models.MeetingPlanner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClosingPrayer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClosingSong")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConductingLeader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IntermediateNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MeetingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpeningPrayer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OpeningSong")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SacramentHymn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MeetingPlanner");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Speaker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MeetingPlannerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MeetingPlannerId");

                    b.ToTable("Speaker");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Speaker", b =>
                {
                    b.HasOne("SacramentMeetingPlanner.Models.MeetingPlanner", null)
                        .WithMany("Speakers")
                        .HasForeignKey("MeetingPlannerId");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.MeetingPlanner", b =>
                {
                    b.Navigation("Speakers");
                });
#pragma warning restore 612, 618
        }
    }
}