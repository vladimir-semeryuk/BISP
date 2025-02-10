﻿// <auto-generated />
using System;
using EchoesOfUzbekistan.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EchoesOfUzbekistan.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250208053655_AddUserIdentityIdColumn")]
    partial class AddUserIdentityIdColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AudioGuidePlace", b =>
                {
                    b.Property<Guid>("AudioGuideId")
                        .HasColumnType("uuid")
                        .HasColumnName("audio_guide_id");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uuid")
                        .HasColumnName("place_id");

                    b.HasKey("AudioGuideId", "PlaceId")
                        .HasName("pk_audio_guide_place");

                    b.HasIndex("PlaceId")
                        .HasDatabaseName("ix_audio_guide_place_place_id");

                    b.ToTable("audio_guide_place", (string)null);
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Common.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character(2)")
                        .HasColumnName("code")
                        .IsFixedLength();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_languages");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_languages_code");

                    b.ToTable("languages", (string)null);
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Guides.AudioGuide", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AudioLink")
                        .HasColumnType("text")
                        .HasColumnName("audio_link");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("City")
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<DateTime?>("DateEdited")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_edited");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_published");

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)")
                        .HasColumnName("description");

                    b.Property<string>("ImageLink")
                        .HasColumnType("text")
                        .HasColumnName("image_link");

                    b.Property<Guid>("OriginalLanguageId")
                        .HasColumnType("uuid")
                        .HasColumnName("original_language_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_audio_guides");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_audio_guides_author_id");

                    b.HasIndex("OriginalLanguageId")
                        .HasDatabaseName("ix_audio_guides_original_language_id");

                    b.ToTable("audio_guides", (string)null);
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Places.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AudioGuideId")
                        .HasColumnType("uuid")
                        .HasColumnName("audio_guide_id");

                    b.Property<string>("AudioLink")
                        .HasColumnType("text")
                        .HasColumnName("audio_link");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Point>("Coordinates")
                        .IsRequired()
                        .HasColumnType("geography(Point, 4326)")
                        .HasColumnName("coordinates");

                    b.Property<DateTime?>("DateEdited")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_edited");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_published");

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)")
                        .HasColumnName("description");

                    b.Property<string>("ImageLink")
                        .HasColumnType("text")
                        .HasColumnName("image_link");

                    b.Property<Guid>("OriginalLanguageId")
                        .HasColumnType("uuid")
                        .HasColumnName("original_language_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_place");

                    b.HasIndex("AudioGuideId")
                        .HasDatabaseName("ix_place_audio_guide_id");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_place_author_id");

                    b.HasIndex("OriginalLanguageId")
                        .HasDatabaseName("ix_place_original_language_id");

                    b.ToTable("place", (string)null);
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Routes.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AudioGuideId")
                        .HasColumnType("uuid")
                        .HasColumnName("audio_guide_id");

                    b.Property<LineString>("RouteLine")
                        .HasColumnType("geometry")
                        .HasColumnName("route_line");

                    b.HasKey("Id")
                        .HasName("pk_routes");

                    b.HasIndex("AudioGuideId")
                        .HasDatabaseName("ix_routes_audio_guide_id");

                    b.ToTable("routes", (string)null);
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AboutMe")
                        .HasColumnType("text")
                        .HasColumnName("about_me");

                    b.Property<string>("City")
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_id");

                    b.Property<DateTime>("RegistrationDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registration_date_utc");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("IdentityId")
                        .IsUnique()
                        .HasDatabaseName("ix_users_identity_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("AudioGuidePlace", b =>
                {
                    b.HasOne("EchoesOfUzbekistan.Domain.Guides.AudioGuide", null)
                        .WithMany()
                        .HasForeignKey("AudioGuideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_audio_guide_place_audio_guide_audio_guide_id");

                    b.HasOne("EchoesOfUzbekistan.Domain.Places.Place", null)
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_audio_guide_place_place_place_id");
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Guides.AudioGuide", b =>
                {
                    b.HasOne("EchoesOfUzbekistan.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_audio_guides_user_author_id");

                    b.HasOne("EchoesOfUzbekistan.Domain.Common.Language", null)
                        .WithMany()
                        .HasForeignKey("OriginalLanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_audio_guides_language_original_language_id");

                    b.OwnsOne("EchoesOfUzbekistan.Domain.Common.Money", "Price", b1 =>
                        {
                            b1.Property<Guid>("AudioGuideId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("price_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("price_currency");

                            b1.HasKey("AudioGuideId");

                            b1.ToTable("audio_guides");

                            b1.WithOwner()
                                .HasForeignKey("AudioGuideId")
                                .HasConstraintName("fk_audio_guide_audio_guide_id");
                        });

                    b.OwnsMany("EchoesOfUzbekistan.Domain.Guides.GuideTranslation", "Translations", b1 =>
                        {
                            b1.Property<Guid>("AudioGuideId")
                                .HasColumnType("uuid")
                                .HasColumnName("audio_guide_id");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasColumnName("id");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("description")
                                .HasColumnType("text")
                                .HasColumnName("description");

                            b1.Property<Guid>("languageId")
                                .HasColumnType("uuid")
                                .HasColumnName("language_id");

                            b1.Property<string>("title")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("title");

                            b1.HasKey("AudioGuideId", "Id")
                                .HasName("pk_guide_translation");

                            b1.HasIndex("languageId")
                                .HasDatabaseName("ix_guide_translation_language_id");

                            b1.ToTable("guide_translation", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AudioGuideId")
                                .HasConstraintName("fk_guide_translation_audio_guide_audio_guide_id");

                            b1.HasOne("EchoesOfUzbekistan.Domain.Common.Language", null)
                                .WithMany()
                                .HasForeignKey("languageId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired()
                                .HasConstraintName("fk_guide_translation_language_language_id");
                        });

                    b.Navigation("Price")
                        .IsRequired();

                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Places.Place", b =>
                {
                    b.HasOne("EchoesOfUzbekistan.Domain.Guides.AudioGuide", null)
                        .WithMany("Places")
                        .HasForeignKey("AudioGuideId")
                        .HasConstraintName("fk_place_audio_guide_audio_guide_id");

                    b.HasOne("EchoesOfUzbekistan.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_place_user_author_id");

                    b.HasOne("EchoesOfUzbekistan.Domain.Common.Language", null)
                        .WithMany()
                        .HasForeignKey("OriginalLanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_place_languages_original_language_id");

                    b.OwnsMany("EchoesOfUzbekistan.Domain.Places.PlaceTranslation", "Translations", b1 =>
                        {
                            b1.Property<Guid>("placeId")
                                .HasColumnType("uuid")
                                .HasColumnName("place_id");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasColumnName("id");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("description")
                                .HasColumnType("text")
                                .HasColumnName("description");

                            b1.Property<Guid>("languageId")
                                .HasColumnType("uuid")
                                .HasColumnName("language_id");

                            b1.Property<string>("title")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("title");

                            b1.HasKey("placeId", "Id")
                                .HasName("pk_place_translation");

                            b1.HasIndex("languageId")
                                .HasDatabaseName("ix_place_translation_language_id");

                            b1.ToTable("place_translation", (string)null);

                            b1.HasOne("EchoesOfUzbekistan.Domain.Common.Language", null)
                                .WithMany()
                                .HasForeignKey("languageId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired()
                                .HasConstraintName("fk_place_translation_languages_language_id");

                            b1.WithOwner()
                                .HasForeignKey("placeId")
                                .HasConstraintName("fk_place_translation_place_place_id");
                        });

                    b.Navigation("Translations");
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Routes.Route", b =>
                {
                    b.HasOne("EchoesOfUzbekistan.Domain.Guides.AudioGuide", null)
                        .WithMany()
                        .HasForeignKey("AudioGuideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_routes_audio_guides_audio_guide_id");
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Users.User", b =>
                {
                    b.OwnsOne("EchoesOfUzbekistan.Domain.Common.Country", "Country", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("IsoCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("country_iso_code");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("country_name");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_user_user_id");
                        });

                    b.Navigation("Country")
                        .IsRequired();
                });

            modelBuilder.Entity("EchoesOfUzbekistan.Domain.Guides.AudioGuide", b =>
                {
                    b.Navigation("Places");
                });
#pragma warning restore 612, 618
        }
    }
}
