﻿// <auto-generated />
using System;
using ATM.Simulates.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ATM.Simulates.API.Migrations
{
    [DbContext(typeof(ATMContext))]
    [Migration("20190826060741_TraceLogin")]
    partial class TraceLogin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ATM.Simulates.API.Models.Account", b =>
                {
                    b.Property<long>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName")
                        .HasMaxLength(50);

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("isLock");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ATM.Simulates.API.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<string>("Status");

                    b.Property<DateTime>("TransDate");

                    b.Property<int>("TransactionTypeId");

                    b.Property<Guid>("WalletDesId");

                    b.Property<Guid>("WalletSourceId");

                    b.HasKey("TransactionId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ATM.Simulates.API.Models.TransactionLog", b =>
                {
                    b.Property<int>("TransactionLognId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccountId");

                    b.Property<decimal>("BalanceAfter");

                    b.Property<decimal>("BalanceBefore");

                    b.Property<string>("Status");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<int>("TransactionId");

                    b.Property<int>("TransactionTypeId");

                    b.HasKey("TransactionLognId");

                    b.ToTable("TransactionLogs");
                });

            modelBuilder.Entity("ATM.Simulates.API.Models.TransactionType", b =>
                {
                    b.Property<int>("TransactionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Fee");

                    b.Property<string>("TransactionTypeName");

                    b.HasKey("TransactionTypeId");

                    b.ToTable("TransactionType");
                });

            modelBuilder.Entity("ATM.Simulates.API.Models.Wallet", b =>
                {
                    b.Property<Guid>("WalletId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccountId");

                    b.Property<decimal>("Balance");

                    b.Property<int>("WalletTypeId");

                    b.HasKey("WalletId");

                    b.HasIndex("AccountId");

                    b.HasIndex("WalletTypeId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("ATM.Simulates.API.Models.WalletType", b =>
                {
                    b.Property<int>("WalletTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("WalletTypeName");

                    b.HasKey("WalletTypeId");

                    b.ToTable("WalletType");
                });

            modelBuilder.Entity("ATM.Simulates.API.Models.Wallet", b =>
                {
                    b.HasOne("ATM.Simulates.API.Models.Account", "Account")
                        .WithMany("Wallet")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ATM.Simulates.API.Models.WalletType", "WalletType")
                        .WithMany()
                        .HasForeignKey("WalletTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
