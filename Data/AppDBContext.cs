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

            modelBuilder.Entity<Game>().HasData(
        new Game
        {
            Id = 1,
            GameName = "CS2",
            GameType = "FPS",
            MaxPlayers = 5,
            MinPlayers = 1
        },
        new Game
        {
            Id = 2,
            GameName = "Valorant",
            GameType = "FPS",
            MaxPlayers = 5,
            MinPlayers = 1
        },
        new Game
        {
            Id = 3,
            GameName = "Overwatch",
            GameType = "FPS",
            MaxPlayers = 5,
            MinPlayers = 1
        },
        new Game
        {
            Id = 4,
            GameName = "LoL",
            GameType = "MOBA",
            MaxPlayers = 5,
            MinPlayers = 1
        },
        new Game
        {
            Id = 5,
            GameName = "Mobile Legends",
            GameType = "MOBA",
            MaxPlayers = 5,
            MinPlayers = 1
        },
        new Game
        {
            Id = 6,
            GameName = "RoV",
            GameType = "MOBA",
            MaxPlayers = 5,
            MinPlayers = 1
        },
        new Game
        {
            Id = 7,
            GameName = "Among Us",
            GameType = "Party",
            MaxPlayers = 15,
            MinPlayers = 4
        },
        new Game
        {
            Id = 8,
            GameName = "Peak",
            GameType = "Party",
            MaxPlayers = 4,
            MinPlayers = 1
        },
        new Game
        {
            Id = 9,
            GameName = "PUBG",
            GameType = "Battle Royale",
            MaxPlayers = 4,
            MinPlayers = 1
        }
    );

            // Seed GameRoles
            modelBuilder.Entity<GameRole>().HasData(
                // Valorant Roles (GameId = 2)
                new GameRole { Id = 1, GameId = 2, RoleName = "Duelist" },
                new GameRole { Id = 2, GameId = 2, RoleName = "Controller" },
                new GameRole { Id = 3, GameId = 2, RoleName = "Initiator" },
                new GameRole { Id = 4, GameId = 2, RoleName = "Sentinel" },

                // LOL Roles (GameId = 4)
                new GameRole { Id = 5, GameId = 4, RoleName = "Top Lane" },
                new GameRole { Id = 6, GameId = 4, RoleName = "Jungle" },
                new GameRole { Id = 7, GameId = 4, RoleName = "Mid Lane" },
                new GameRole { Id = 8, GameId = 4, RoleName = "ADC" },
                new GameRole { Id = 9, GameId = 4, RoleName = "Support" },

                // RoV Roles (GameId = 6)
                new GameRole { Id = 10, GameId = 6, RoleName = "Top Lane" },
                new GameRole { Id = 11, GameId = 6, RoleName = "Jungle" },
                new GameRole { Id = 12, GameId = 6, RoleName = "Mid Lane" },
                new GameRole { Id = 13, GameId = 6, RoleName = "ADC" },
                new GameRole { Id = 14, GameId = 6, RoleName = "Support" }
            );

            // Seed GameRanks (Valorant - GameId = 2)
            modelBuilder.Entity<GameRank>().HasData(
                new GameRank { Id = 1, GameId = 2, RankName = "Iron", RankImageUrl = "/images/ranks/val/iron.png" },
                new GameRank { Id = 2, GameId = 2, RankName = "Bronze", RankImageUrl = "/images/ranks/val/bronze.png" },
                new GameRank { Id = 3, GameId = 2, RankName = "Silver", RankImageUrl = "/images/ranks/val/silver.png" },
                new GameRank { Id = 4, GameId = 2, RankName = "Gold", RankImageUrl = "/images/ranks/val/gold.png" },
                new GameRank { Id = 5, GameId = 2, RankName = "Platinum", RankImageUrl = "/images/ranks/val/plat.png" },
                new GameRank { Id = 6, GameId = 2, RankName = "Diamond", RankImageUrl = "/images/ranks/val/diamond.png" },
                new GameRank { Id = 7, GameId = 2, RankName = "Immortal", RankImageUrl = "/images/ranks/val/immortal.png" },
                new GameRank { Id = 8, GameId = 2, RankName = "Radiant", RankImageUrl = "/images/ranks/val/radiant.png" }
            );

        }
    }
}