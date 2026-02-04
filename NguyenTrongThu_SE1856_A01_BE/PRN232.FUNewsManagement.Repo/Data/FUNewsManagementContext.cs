using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Data
{
    public class FUNewsManagementContext : DbContext
    {
        public FUNewsManagementContext(DbContextOptions<FUNewsManagementContext> options)
            : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SystemAccount configuration
            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.ToTable("SystemAccount");
                entity.HasKey(e => e.AccountID);
                entity.Property(e => e.AccountID).UseIdentityColumn();
                entity.Property(e => e.AccountName).HasMaxLength(100);
                entity.Property(e => e.AccountEmail).HasMaxLength(100);
                entity.HasIndex(e => e.AccountEmail).IsUnique();
                entity.Property(e => e.AccountPassword).HasMaxLength(100);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryID).UseIdentityColumn();
                entity.Property(e => e.CategoryName).HasMaxLength(100);
                entity.Property(e => e.CategoryDesciption).HasMaxLength(250);
                
                entity.HasOne(e => e.ParentCategory)
                    .WithMany(e => e.ChildCategories)
                    .HasForeignKey(e => e.ParentCategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // NewsArticle configuration
            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.ToTable("NewsArticle");
                entity.HasKey(e => e.NewsArticleID);
                entity.Property(e => e.NewsArticleID).HasMaxLength(20);
                entity.Property(e => e.NewsTitle).HasMaxLength(400);
                entity.Property(e => e.Headline).HasMaxLength(150);
                entity.Property(e => e.NewsSource).HasMaxLength(400);
                
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.NewsArticles)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.CreatedBy)
                    .WithMany(e => e.CreatedNewsArticles)
                    .HasForeignKey(e => e.CreatedByID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Tag configuration
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
                entity.HasKey(e => e.TagID);
                entity.Property(e => e.TagID).UseIdentityColumn();
                entity.Property(e => e.TagName).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(400);
            });

            // NewsTag configuration
            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.ToTable("NewsTag");
                entity.HasKey(e => new { e.NewsArticleID, e.TagID });
                
                entity.HasOne(e => e.NewsArticle)
                    .WithMany(e => e.NewsTags)
                    .HasForeignKey(e => e.NewsArticleID)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Tag)
                    .WithMany(e => e.NewsTags)
                    .HasForeignKey(e => e.TagID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
