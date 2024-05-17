using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HardPedia.Models.Domain;

namespace HardPedia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Categories)
                .WithMany(c => c.Subjects)
                .UsingEntity<Dictionary<string, object>>(
                    "SubjectCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Subject>().WithMany().HasForeignKey("SubjectId"));

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(s => s.Heading).IsRequired();
                entity.Property(s => s.Title).IsRequired();
                entity.Property(s => s.Content).IsRequired();
                entity.Property(s => s.Author).IsRequired();
                entity.Property(s => s.Visible).HasDefaultValue(true);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).IsRequired();
                entity.HasIndex(c => c.Name).IsUnique();
                entity.HasMany(c => c.Subjects)
                      .WithMany(s => s.Categories)
                      .UsingEntity(j => j.ToTable("SubjectCategory"));
            });
        }
    }
}
