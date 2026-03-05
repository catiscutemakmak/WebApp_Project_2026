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

        // Room System Tables
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPlayer> RoomPlayers { get; set; }
        public DbSet<RoomQueue> RoomQueues { get; set; }
        public DbSet<RoomChat> RoomChats { get; set; }
        public DbSet<RoomSetting> RoomSettings { get; set; }

        public DbSet<UserGame> UserGames { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.UserId)  // ← Query เร็วขึ้นมาก
                .IsUnique(); 

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

            // Room relationships
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Game)
                .WithMany()
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomOwner)
                .WithMany()
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // RoomPlayer relationships
            modelBuilder.Entity<RoomPlayer>()
                .HasOne(rp => rp.Room)
                .WithMany(r => r.Players)
                .HasForeignKey(rp => rp.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomPlayer>()
                .HasOne(rp => rp.User)
                .WithMany()
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // RoomQueue relationships
            modelBuilder.Entity<RoomQueue>()
                .HasOne(rq => rq.Room)
                .WithMany(r => r.QueuePlayers)
                .HasForeignKey(rq => rq.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomQueue>()
                .HasOne(rq => rq.User)
                .WithMany()
                .HasForeignKey(rq => rq.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // RoomChat relationships
            modelBuilder.Entity<RoomChat>()
                .HasOne(rc => rc.Room)
                .WithMany(r => r.Chats)
                .HasForeignKey(rc => rc.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomChat>()
                .HasOne(rc => rc.User)
                .WithMany()
                .HasForeignKey(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // RoomSetting relationship (1-to-1)
            modelBuilder.Entity<RoomSetting>()
                .HasOne(rs => rs.Room)
                .WithOne(r => r.RoomSetting)
                .HasForeignKey<RoomSetting>(rs => rs.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

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
                new GameRank { Id = 1, GameId = 2, RankName = "Iron", RankImageUrl = "/images/ranks/val/Iron.webp" },
                new GameRank { Id = 2, GameId = 2, RankName = "Bronze", RankImageUrl = "/images/ranks/val/Bronze.webp" },
                new GameRank { Id = 3, GameId = 2, RankName = "Silver", RankImageUrl = "/images/ranks/val/Silver.webp" },
                new GameRank { Id = 4, GameId = 2, RankName = "Gold", RankImageUrl = "/images/ranks/val/Gold.webp" },
                new GameRank { Id = 5, GameId = 2, RankName = "Platinum", RankImageUrl = "/images/ranks/val/Platinum.webp" },
                new GameRank { Id = 6, GameId = 2, RankName = "Diamond", RankImageUrl = "/images/ranks/val/Diamond.webp" },
                new GameRank { Id = 7, GameId = 2, RankName = "Immortal", RankImageUrl = "/images/ranks/val/Immortal.webp" },
                new GameRank { Id = 8, GameId = 2, RankName = "Radiant", RankImageUrl = "/images/ranks/val/Radiant.webp" }
            );

        }
    }
}