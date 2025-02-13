﻿// <auto-generated />
using System;
using Api_Bot_RPG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api_Bot_RPG.Migrations
{
    [DbContext(typeof(RpgContext))]
    [Migration("20250210122048_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Api_Bot_RPG.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Agilite")
                        .HasColumnType("int");

                    b.Property<int>("Alignement")
                        .HasColumnType("int");

                    b.Property<int>("Chance")
                        .HasColumnType("int");

                    b.Property<string>("Classe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Defense")
                        .HasColumnType("int");

                    b.Property<int>("Energie")
                        .HasColumnType("int");

                    b.Property<int>("EnergieMax")
                        .HasColumnType("int");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int>("Force")
                        .HasColumnType("int");

                    b.Property<int>("Mana")
                        .HasColumnType("int");

                    b.Property<int>("ManaMax")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Niveau")
                        .HasColumnType("int");

                    b.Property<int>("PV")
                        .HasColumnType("int");

                    b.Property<int>("PVMax")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PointsDeCompetences")
                        .HasColumnType("int");

                    b.Property<string>("Race")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Vitesse")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessoireId")
                        .HasColumnType("int");

                    b.Property<int>("ArmeId")
                        .HasColumnType("int");

                    b.Property<int>("ArmureId")
                        .HasColumnType("int");

                    b.Property<int>("BottesId")
                        .HasColumnType("int");

                    b.Property<int>("CasqueId")
                        .HasColumnType("int");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccessoireId");

                    b.HasIndex("ArmeId");

                    b.HasIndex("ArmureId");

                    b.HasIndex("BottesId");

                    b.HasIndex("CasqueId");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CharacterId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantite")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DiscordId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxCharacters")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CharacterId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EstDebloque")
                        .HasColumnType("bit");

                    b.Property<int>("Niveau")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EffectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillNodeId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillNodeId");

                    b.ToTable("SkillEffects");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CoutPoints")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrerequisSkillIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SkillTreeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillTreeId");

                    b.ToTable("SkillNodes");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillTree", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Classe")
                        .HasColumnType("int");

                    b.Property<int>("Race")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SkillTrees");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Character", b =>
                {
                    b.HasOne("Api_Bot_RPG.Models.Player", null)
                        .WithMany("Characters")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Equipment", b =>
                {
                    b.HasOne("Api_Bot_RPG.Models.Item", "Accessoire")
                        .WithMany()
                        .HasForeignKey("AccessoireId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Api_Bot_RPG.Models.Item", "Arme")
                        .WithMany()
                        .HasForeignKey("ArmeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Api_Bot_RPG.Models.Item", "Armure")
                        .WithMany()
                        .HasForeignKey("ArmureId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Api_Bot_RPG.Models.Item", "Bottes")
                        .WithMany()
                        .HasForeignKey("BottesId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Api_Bot_RPG.Models.Item", "Casque")
                        .WithMany()
                        .HasForeignKey("CasqueId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Api_Bot_RPG.Models.Character", "Character")
                        .WithOne("Equipements")
                        .HasForeignKey("Api_Bot_RPG.Models.Equipment", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accessoire");

                    b.Navigation("Arme");

                    b.Navigation("Armure");

                    b.Navigation("Bottes");

                    b.Navigation("Casque");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Item", b =>
                {
                    b.HasOne("Api_Bot_RPG.Models.Character", null)
                        .WithMany("Inventaire")
                        .HasForeignKey("CharacterId");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Skill", b =>
                {
                    b.HasOne("Api_Bot_RPG.Models.Character", null)
                        .WithMany("Competences")
                        .HasForeignKey("CharacterId");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillEffect", b =>
                {
                    b.HasOne("Api_Bot_RPG.Models.SkillNode", "SkillNode")
                        .WithMany("Effets")
                        .HasForeignKey("SkillNodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SkillNode");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillNode", b =>
                {
                    b.HasOne("Api_Bot_RPG.Models.SkillTree", null)
                        .WithMany("Nodes")
                        .HasForeignKey("SkillTreeId");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Character", b =>
                {
                    b.Navigation("Competences");

                    b.Navigation("Equipements")
                        .IsRequired();

                    b.Navigation("Inventaire");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.Player", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillNode", b =>
                {
                    b.Navigation("Effets");
                });

            modelBuilder.Entity("Api_Bot_RPG.Models.SkillTree", b =>
                {
                    b.Navigation("Nodes");
                });
#pragma warning restore 612, 618
        }
    }
}
