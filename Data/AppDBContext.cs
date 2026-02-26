using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using hateekub.Models;


namespace hateekub.Data
{

    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameRole> GameRoles { get; set; }
        public DbSet<GameRank> GameRanks { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // กำหนด Review relationships อย่างชัดเจน
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)                    // Review.User
                .WithMany(u => u.Reviews)               // UserProfile.Reviews
                .HasForeignKey(r => r.UserProfileId)    // ใช้ UserProfileId
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)                // Review.Reviewer
                .WithMany()                             // ไม่มี collection ใน UserProfile
                .HasForeignKey(r => r.ReviewerId)       // ใช้ ReviewerId
                .OnDelete(DeleteBehavior.Restrict);     // ห้ามลบถ้ามี Review

                

        }
    }
}