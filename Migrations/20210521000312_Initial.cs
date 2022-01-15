using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wiki.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Word = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 20, nullable: false),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    PrimaryCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_PrimaryCategoryId",
                        column: x => x.PrimaryCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticlesCategories",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesCategories", x => new { x.ArticleId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ArticlesCategories_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArticlesCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticlesKeyWords",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(nullable: false),
                    KeyWordId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesKeyWords", x => new { x.ArticleId, x.KeyWordId });
                    table.ForeignKey(
                        name: "FK_ArticlesKeyWords_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArticlesKeyWords_KeyWords_KeyWordId",
                        column: x => x.KeyWordId,
                        principalTable: "KeyWords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bodies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bodies_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Headers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    ArticleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headers_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Text = table.Column<string>(maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    MainArticleId = table.Column<Guid>(nullable: false),
                    ReferenceArticleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => new { x.MainArticleId, x.ReferenceArticleId });
                    table.ForeignKey(
                        name: "FK_References_Articles_MainArticleId",
                        column: x => x.MainArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_References_Articles_ReferenceArticleId",
                        column: x => x.ReferenceArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Subtitle = table.Column<string>(maxLength: 50, nullable: true),
                    Text = table.Column<string>(maxLength: 200, nullable: false),
                    BodyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_Bodies_BodyId",
                        column: x => x.BodyId,
                        principalTable: "Bodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PrimaryCategoryId",
                table: "Articles",
                column: "PrimaryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesCategories_CategoryId",
                table: "ArticlesCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesKeyWords_KeyWordId",
                table: "ArticlesKeyWords",
                column: "KeyWordId");

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_ArticleId",
                table: "Bodies",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BodyId",
                table: "Entries",
                column: "BodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Headers_ArticleId",
                table: "Headers",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ArticleId",
                table: "Messages",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_References_ReferenceArticleId",
                table: "References",
                column: "ReferenceArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticlesCategories");

            migrationBuilder.DropTable(
                name: "ArticlesKeyWords");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Headers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "KeyWords");

            migrationBuilder.DropTable(
                name: "Bodies");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
