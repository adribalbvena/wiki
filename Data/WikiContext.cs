using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;

using Wiki.Models;

namespace Wiki.Data
{
    public class WikiContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public WikiContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.DefineUsersRoles(modelBuilder);
            this.DefineArticlesCategories(modelBuilder);
            this.DefineArticlesKeyWords(modelBuilder);
            this.DefineReferences(modelBuilder);
            this.DefineMessages(modelBuilder);
            this.DefineArticles(modelBuilder);
        }

        private void DefineUsersRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser<Guid>>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UsersRoles");
        }

        private void DefineArticles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasOne(a => a.PrimaryCategory)
                .WithMany(c => c.PrimaryArticles)
                .HasForeignKey(a => a.PrimaryCategoryId);
        }

        private void DefineMessages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Article)
                .WithMany(a => a.Messages)
                .HasForeignKey(m => m.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void DefineReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reference>().ToTable("References");

            modelBuilder.Entity<Reference>().HasKey(r => new { r.MainArticleId, r.ReferenceArticleId });

            modelBuilder.Entity<Reference>()
                .HasOne(ac => ac.MainArticle)
                .WithMany(a => a.MainReferences)
                .HasForeignKey(ac => ac.MainArticleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reference>()
                .HasOne(ac => ac.ReferenceArticle)
                .WithMany(a => a.References)
                .HasForeignKey(ac => ac.ReferenceArticleId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void DefineArticlesCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleCategory>().ToTable("ArticlesCategories");

            modelBuilder.Entity<ArticleCategory>().HasKey(ac => new { ac.ArticleId, ac.CategoryId });

            modelBuilder.Entity<ArticleCategory>()
                .HasOne(ac => ac.Article)
                .WithMany(a => a.SecondaryCategories)
                .HasForeignKey(ac => ac.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ArticleCategory>()
                .HasOne(ac => ac.Category)
                .WithMany(a => a.SecondaryArticles)
                .HasForeignKey(ac => ac.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private void DefineArticlesKeyWords(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleKeyWord>().ToTable("ArticlesKeyWords");

            modelBuilder.Entity<ArticleKeyWord>().HasKey(ac => new { ac.ArticleId, ac.KeyWordId });

            modelBuilder.Entity<ArticleKeyWord>()
                .HasOne(ac => ac.Article)
                .WithMany(a => a.KeyWords)
                .HasForeignKey(ac => ac.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ArticleKeyWord>()
                .HasOne(ac => ac.KeyWord)
                .WithMany(a => a.Articles)
                .HasForeignKey(ac => ac.KeyWordId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Reference> References { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ArticleCategory> ArticlesCategories { get; set; }

        public DbSet<KeyWord> KeyWords { get; set; }

        public DbSet<ArticleKeyWord> ArticlesKeyWords { get; set; }

        public DbSet<Entry> Entries { get; set; }

        public DbSet<Body> Bodies { get; set; }

        public DbSet<Header> Headers { get; set; }
    }
}
