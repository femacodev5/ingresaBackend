﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ingresa.Context;

#nullable disable

namespace ingresa.Migrations
{
    [DbContext(typeof(AppDBcontext))]
    [Migration("20240604133444_17")]
    partial class _17
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ingresa.Models.Cluster", b =>
                {
                    b.Property<int>("ClusterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClusterId"));

                    b.Property<string>("ClusterName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClusterId");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("ingresa.Models.Contract", b =>
                {
                    b.Property<int>("ContractId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractId"));

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("FechaConclucionContrato")
                        .HasColumnType("date");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.Property<int>("Vacation")
                        .HasColumnType("int");

                    b.HasKey("ContractId");

                    b.HasIndex("PersonId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("ingresa.Models.ContractFile", b =>
                {
                    b.Property<int>("ContractFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContractFileId"));

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("FileContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContractFileId");

                    b.HasIndex("ContractId");

                    b.ToTable("ContractFiles");
                });

            modelBuilder.Entity("ingresa.Models.DialingRecord", b =>
                {
                    b.Property<int>("DialingRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DialingRecordId"));

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("DialingRecordId");

                    b.HasIndex("PersonId");

                    b.ToTable("DialingRecords");
                });

            modelBuilder.Entity("ingresa.Models.Holiday", b =>
                {
                    b.Property<int>("HolidayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HolidayId"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HolidayId");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("ingresa.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<int>("ClusterId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("PersonId1")
                        .HasColumnType("int");

                    b.Property<int?>("ShiftId")
                        .HasColumnType("int");

                    b.HasKey("PersonId");

                    b.HasIndex("ClusterId");

                    b.HasIndex("PersonId1");

                    b.HasIndex("ShiftId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("ingresa.Models.Shift", b =>
                {
                    b.Property<int>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShiftId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShiftId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("ingresa.Models.ShiftDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DiaSemana")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("FinMarcacionDescanso")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("FinMarcacionEntrada")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("FinMarcacionSalida")
                        .HasColumnType("time");

                    b.Property<bool>("HabilitarDescanso")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("HoraEntrada")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("HoraSalida")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("InicioMarcacionDescanso")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("InicioMarcacionEntrada")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("InicioMarcacionSalida")
                        .HasColumnType("time");

                    b.Property<int>("MinutosDescanso")
                        .HasColumnType("int");

                    b.Property<int>("MinutosJornada")
                        .HasColumnType("int");

                    b.Property<int>("MinutosJornadaNeto")
                        .HasColumnType("int");

                    b.Property<int>("ShiftId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.ToTable("ShiftDetails");
                });

            modelBuilder.Entity("ingresa.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("PersonId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ingresa.Models.Contract", b =>
                {
                    b.HasOne("ingresa.Models.Person", "Person")
                        .WithMany("Contracts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("ingresa.Models.ContractFile", b =>
                {
                    b.HasOne("ingresa.Models.Contract", "Contract")
                        .WithMany("ContractFiles")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("ingresa.Models.DialingRecord", b =>
                {
                    b.HasOne("ingresa.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("ingresa.Models.Person", b =>
                {
                    b.HasOne("ingresa.Models.Cluster", "Cluster")
                        .WithMany("Person")
                        .HasForeignKey("ClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ingresa.Models.Person", null)
                        .WithMany("Persons")
                        .HasForeignKey("PersonId1");

                    b.HasOne("ingresa.Models.Shift", "Shift")
                        .WithMany("Persons")
                        .HasForeignKey("ShiftId");

                    b.Navigation("Cluster");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ingresa.Models.ShiftDetail", b =>
                {
                    b.HasOne("ingresa.Models.Shift", "Shift")
                        .WithMany("ShiftDetails")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ingresa.Models.User", b =>
                {
                    b.HasOne("ingresa.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("ingresa.Models.Cluster", b =>
                {
                    b.Navigation("Person");
                });

            modelBuilder.Entity("ingresa.Models.Contract", b =>
                {
                    b.Navigation("ContractFiles");
                });

            modelBuilder.Entity("ingresa.Models.Person", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Persons");
                });

            modelBuilder.Entity("ingresa.Models.Shift", b =>
                {
                    b.Navigation("Persons");

                    b.Navigation("ShiftDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
