using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EchoesOfUzbekistan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslationsAudio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "audio_link",
                table: "place_translation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "audio_link",
                table: "guide_translation",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "audio_link",
                table: "place_translation");

            migrationBuilder.DropColumn(
                name: "audio_link",
                table: "guide_translation");
        }
    }
}
