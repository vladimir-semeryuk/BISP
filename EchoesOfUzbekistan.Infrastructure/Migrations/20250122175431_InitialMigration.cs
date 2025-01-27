using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EchoesOfUzbekistan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    registration_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    country_name = table.Column<string>(type: "text", nullable: false),
                    country_iso_code = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: true),
                    about_me = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audio_guides",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    price_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    price_currency = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    date_published = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audio_link = table.Column<string>(type: "text", nullable: true),
                    image_link = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audio_guides", x => x.id);
                    table.ForeignKey(
                        name: "fk_audio_guides_language_original_language_id",
                        column: x => x.original_language_id,
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_audio_guides_user_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "guide_translation",
                columns: table => new
                {
                    audio_guide_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guide_translation", x => new { x.audio_guide_id, x.id });
                    table.ForeignKey(
                        name: "fk_guide_translation_audio_guide_audio_guide_id",
                        column: x => x.audio_guide_id,
                        principalTable: "audio_guides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_guide_translation_language_language_id",
                        column: x => x.language_id,
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    coordinates = table.Column<Point>(type: "geography(Point, 4326)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    date_published = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audio_link = table.Column<string>(type: "text", nullable: true),
                    image_link = table.Column<string>(type: "text", nullable: true),
                    audio_guide_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place", x => x.id);
                    table.ForeignKey(
                        name: "fk_place_audio_guide_audio_guide_id",
                        column: x => x.audio_guide_id,
                        principalTable: "audio_guides",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_place_languages_original_language_id",
                        column: x => x.original_language_id,
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_place_user_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    audio_guide_id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_line = table.Column<LineString>(type: "geometry", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_routes", x => x.id);
                    table.ForeignKey(
                        name: "fk_routes_audio_guides_audio_guide_id",
                        column: x => x.audio_guide_id,
                        principalTable: "audio_guides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audio_guide_place",
                columns: table => new
                {
                    audio_guide_id = table.Column<Guid>(type: "uuid", nullable: false),
                    place_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audio_guide_place", x => new { x.audio_guide_id, x.place_id });
                    table.ForeignKey(
                        name: "fk_audio_guide_place_audio_guide_audio_guide_id",
                        column: x => x.audio_guide_id,
                        principalTable: "audio_guides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_audio_guide_place_place_place_id",
                        column: x => x.place_id,
                        principalTable: "place",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place_translation",
                columns: table => new
                {
                    place_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place_translation", x => new { x.place_id, x.id });
                    table.ForeignKey(
                        name: "fk_place_translation_languages_language_id",
                        column: x => x.language_id,
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_place_translation_place_place_id",
                        column: x => x.place_id,
                        principalTable: "place",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_audio_guide_place_place_id",
                table: "audio_guide_place",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "ix_audio_guides_author_id",
                table: "audio_guides",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_audio_guides_original_language_id",
                table: "audio_guides",
                column: "original_language_id");

            migrationBuilder.CreateIndex(
                name: "ix_guide_translation_language_id",
                table: "guide_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_languages_code",
                table: "languages",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_place_audio_guide_id",
                table: "place",
                column: "audio_guide_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_author_id",
                table: "place",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_original_language_id",
                table: "place",
                column: "original_language_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_translation_language_id",
                table: "place_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_routes_audio_guide_id",
                table: "routes",
                column: "audio_guide_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audio_guide_place");

            migrationBuilder.DropTable(
                name: "guide_translation");

            migrationBuilder.DropTable(
                name: "place_translation");

            migrationBuilder.DropTable(
                name: "routes");

            migrationBuilder.DropTable(
                name: "place");

            migrationBuilder.DropTable(
                name: "audio_guides");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
