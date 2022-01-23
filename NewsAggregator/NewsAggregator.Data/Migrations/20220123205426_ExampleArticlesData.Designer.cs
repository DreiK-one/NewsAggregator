﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsAggregator.Data;

#nullable disable

namespace NewsAggregator.Data.Migrations
{
    [DbContext(typeof(NewsAggregatorContext))]
    [Migration("20220123205426_ExampleArticlesData")]
    partial class ExampleArticlesData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NewsAggregator.Data.Entities.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Coefficient")
                        .HasColumnType("real");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SourceId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("205e2d6a-76d2-40f1-800b-9d652146c158"),
                            Body = "В последние годы UFC, под эгидой которой проводятся бои по смешанным единоборствам, стабильно держится в топе зрительского интереса. Организация постоянно создает шоу и генерирует эмоции, которые привлекают даже тех, кто не интересуется спортом. Один конкретный бой может раскручиваться как эпохальное событие вселенского масштаба, которое нельзя проигнорировать. При этом непосвященные, включив трансляцию, могут остаться в недоумении: ну, молотят они друг друга в клетке — и что дальше-то? В рамках совместного спортивного спецпроекта Onlíner и  разбираемся, что собой представляет мир UFC и как в него погрузиться.",
                            CategoryId = new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"),
                            Coefficient = 3.5f,
                            CreationDate = new DateTime(2022, 1, 23, 21, 54, 26, 304, DateTimeKind.Local).AddTicks(8838),
                            Description = "Краткий ликбез по смешанным единоборствам",
                            SourceId = new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"),
                            Title = "Как начать смотреть UFC и за кем следить, кроме Макгрегора?"
                        },
                        new
                        {
                            Id = new Guid("248a2c01-569e-4164-bd77-af80e6d89cb8"),
                            Body = "Взять и все поменять — каждый из нас задумывался о таком радикальном шаге хотя бы один раз в жизни. Скорее всего — намного больше. Потому что живем мы как-то неидеально, постоянно совершаем ошибки, не успеем их исправить, как уже новых наделаем — и, закопавшись под всем этим, в какой-то момент приходим в себя уже немолодыми и совсем не счастливыми. ",
                            CategoryId = new Guid("8453b0bd-9860-45fa-a2ae-a77953d842a4"),
                            Coefficient = 4f,
                            CreationDate = new DateTime(2022, 1, 23, 21, 54, 26, 304, DateTimeKind.Local).AddTicks(8897),
                            Description = "Абсолютно счастливый! В 42 года отказался от кресла руководителя крупной строительной компании и стал поваром",
                            SourceId = new Guid("f2fb2a60-c1de-4da5-b047-0871d2d677b5"),
                            Title = "Ушел с хорошей должности и стал поваром!"
                        },
                        new
                        {
                            Id = new Guid("1320bf13-6c2a-4394-aad7-d110ed1c95fc"),
                            Body = "Камерунский и французский боец смешанного стиля (MMA) Фрэнсис Нганну пожаловался на условия контракта с Абсолютным бойцовским чемпионатом (UFC). Об этом сообщает MMA Junkie.",
                            CategoryId = new Guid("ddaea0de-7c80-471f-86c2-b2007d4f8d9e"),
                            Coefficient = 5f,
                            CreationDate = new DateTime(2022, 1, 23, 21, 54, 26, 304, DateTimeKind.Local).AddTicks(8903),
                            Description = "Нганну пожаловался на условия контракта с UFC после защиты титула",
                            SourceId = new Guid("c13088a4-9467-4fce-9ef7-3903425f1f81"),
                            Title = "Чемпион UFC Фрэнсис Нганну о контракте: я не чувствую себя свободным!"
                        });
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RssUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.UserActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ViewingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserActivities");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Article", b =>
                {
                    b.HasOne("NewsAggregator.Data.Entities.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsAggregator.Data.Entities.Source", "Source")
                        .WithMany("Articles")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Comment", b =>
                {
                    b.HasOne("NewsAggregator.Data.Entities.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsAggregator.Data.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.UserActivity", b =>
                {
                    b.HasOne("NewsAggregator.Data.Entities.Article", "Article")
                        .WithMany("UserActivities")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsAggregator.Data.Entities.User", "User")
                        .WithMany("UserActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.UserRole", b =>
                {
                    b.HasOne("NewsAggregator.Data.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsAggregator.Data.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Article", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UserActivities");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Category", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.Source", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("NewsAggregator.Data.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UserActivities");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
