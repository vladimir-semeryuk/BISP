using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EchoesOfUzbekistan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestFriendsStripe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audio_guide_purchases",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guide_id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audio_guide_purchases", x => new { x.user_id, x.guide_id });
                    table.ForeignKey(
                        name: "fk_audio_guide_purchases_audio_guides_guide_id",
                        column: x => x.guide_id,
                        principalTable: "audio_guides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_audio_guide_purchases_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "friendships",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    follower_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_friendships", x => x.id);
                    table.CheckConstraint("CK_Friendship_NoSelfFollow", "\"follower_id\" <> \"followee_id\"");
                    table.ForeignKey(
                        name: "fk_friendships_user_followee_id",
                        column: x => x.followee_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_friendships_user_follower_id",
                        column: x => x.follower_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_audio_guide_purchases_guide_id",
                table: "audio_guide_purchases",
                column: "guide_id");

            migrationBuilder.CreateIndex(
                name: "ix_friendships_followee_id",
                table: "friendships",
                column: "followee_id");

            migrationBuilder.CreateIndex(
                name: "ix_friendships_follower_id_followee_id",
                table: "friendships",
                columns: new[] { "follower_id", "followee_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audio_guide_purchases");

            migrationBuilder.DropTable(
                name: "friendships");
        }
    }
}
