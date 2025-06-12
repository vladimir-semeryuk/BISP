using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EchoesOfUzbekistan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAudioGuidePlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_audio_guide_place_audio_guide_audio_guide_id",
                table: "audio_guide_place");

            migrationBuilder.DropForeignKey(
                name: "fk_audio_guide_place_place_place_id",
                table: "audio_guide_place");

            migrationBuilder.DropForeignKey(
                name: "fk_place_audio_guide_audio_guide_id",
                table: "place");

            migrationBuilder.DropIndex(
                name: "ix_place_audio_guide_id",
                table: "place");

            migrationBuilder.DropColumn(
                name: "audio_guide_id",
                table: "place");

            migrationBuilder.RenameColumn(
                name: "place_id",
                table: "audio_guide_place",
                newName: "places_id");

            migrationBuilder.RenameColumn(
                name: "audio_guide_id",
                table: "audio_guide_place",
                newName: "guides_id");

            migrationBuilder.RenameIndex(
                name: "ix_audio_guide_place_place_id",
                table: "audio_guide_place",
                newName: "ix_audio_guide_place_places_id");

            migrationBuilder.AddForeignKey(
                name: "fk_audio_guide_place_audio_guide_guides_id",
                table: "audio_guide_place",
                column: "guides_id",
                principalTable: "audio_guides",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_audio_guide_place_place_places_id",
                table: "audio_guide_place",
                column: "places_id",
                principalTable: "place",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_audio_guide_place_audio_guide_guides_id",
                table: "audio_guide_place");

            migrationBuilder.DropForeignKey(
                name: "fk_audio_guide_place_place_places_id",
                table: "audio_guide_place");

            migrationBuilder.RenameColumn(
                name: "places_id",
                table: "audio_guide_place",
                newName: "place_id");

            migrationBuilder.RenameColumn(
                name: "guides_id",
                table: "audio_guide_place",
                newName: "audio_guide_id");

            migrationBuilder.RenameIndex(
                name: "ix_audio_guide_place_places_id",
                table: "audio_guide_place",
                newName: "ix_audio_guide_place_place_id");

            migrationBuilder.AddColumn<Guid>(
                name: "audio_guide_id",
                table: "place",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_place_audio_guide_id",
                table: "place",
                column: "audio_guide_id");

            migrationBuilder.AddForeignKey(
                name: "fk_audio_guide_place_audio_guide_audio_guide_id",
                table: "audio_guide_place",
                column: "audio_guide_id",
                principalTable: "audio_guides",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_audio_guide_place_place_place_id",
                table: "audio_guide_place",
                column: "place_id",
                principalTable: "place",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_place_audio_guide_audio_guide_id",
                table: "place",
                column: "audio_guide_id",
                principalTable: "audio_guides",
                principalColumn: "id");
        }
    }
}
