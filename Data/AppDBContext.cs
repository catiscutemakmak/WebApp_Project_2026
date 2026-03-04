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

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGames)
                .HasForeignKey(ug => ug.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(ug => ug.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGame>()
                .HasIndex(ug => new { ug.UserProfileId, ug.GameId })
                .IsUnique();


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
                new GameRole { Id = 14, GameId = 6, RoleName = "Support" },

                // OW Roles (GameId = 3)
                new GameRole { Id = 15, GameId = 3, RoleName = "Tank" },
                new GameRole { Id = 16, GameId = 3, RoleName = "Damage" },
                new GameRole { Id = 17, GameId = 3, RoleName = "Support" }
            );

            // Seed GameRanks
            modelBuilder.Entity<GameRank>().HasData(
                // Valorant Ranks (GameId = 2)
                new GameRank { Id = 1, GameId = 2, RankName = "Iron", RankImageUrl = "/images/ranks/val/Iron.webp" },
                new GameRank { Id = 2, GameId = 2, RankName = "Bronze", RankImageUrl = "/images/ranks/val/Bronze.webp" },
                new GameRank { Id = 3, GameId = 2, RankName = "Silver", RankImageUrl = "/images/ranks/val/Silver.webp" },
                new GameRank { Id = 4, GameId = 2, RankName = "Gold", RankImageUrl = "/images/ranks/val/Gold.webp" },
                new GameRank { Id = 5, GameId = 2, RankName = "Platinum", RankImageUrl = "/images/ranks/val/Platinum.webp" },
                new GameRank { Id = 6, GameId = 2, RankName = "Diamond", RankImageUrl = "/images/ranks/val/Diamond.webp" },
                new GameRank { Id = 7, GameId = 2, RankName = "Immortal", RankImageUrl = "/images/ranks/val/Immortal.webp" },
                new GameRank { Id = 8, GameId = 2, RankName = "Radiant", RankImageUrl = "/images/ranks/val/Radiant.webp" },
                // Mobile Legends Ranks (GameId = 5)
                new GameRank { Id = 9, GameId = 5, RankName = "Warrior", RankImageUrl = "/images/ranks/mlbb/Iron.webp" },
                new GameRank { Id = 10, GameId = 5, RankName = "Elite", RankImageUrl = "/images/ranks/mlbb/Elite.webp" },
                new GameRank { Id = 11, GameId = 5, RankName = "Master", RankImageUrl = "/images/ranks/mlbb/Master.webp" },
                new GameRank { Id = 12, GameId = 5, RankName = "Grandmaster", RankImageUrl = "/images/ranks/mlbb/Grandmaster.webp" },
                new GameRank { Id = 13, GameId = 5, RankName = "Epic", RankImageUrl = "/images/ranks/mlbb/Epic.webp" },
                new GameRank { Id = 14, GameId = 5, RankName = "Legend", RankImageUrl = "/images/ranks/mlbb/Legend.webp" },
                new GameRank { Id = 15, GameId = 5, RankName = "Mythic", RankImageUrl = "/images/ranks/mlbb/Mythic.webp" },

                // Overwatch Ranks (GameId = 3)
                new GameRank { Id = 16, GameId = 3, RankName = "Bronze", RankImageUrl = "/images/ranks/Ow2/Bronze.webp" },
                new GameRank { Id = 17, GameId = 3, RankName = "Silver", RankImageUrl = "/images/ranks/Ow2/Silver.webp" },
                new GameRank { Id = 18, GameId = 3, RankName = "Gold", RankImageUrl = "/images/ranks/Ow2/Gold.webp" },
                new GameRank { Id = 19, GameId = 3, RankName = "Platinum", RankImageUrl = "/images/ranks/Ow2/Platinum.webp" },
                new GameRank { Id = 20, GameId = 3, RankName = "Diamond", RankImageUrl = "/images/ranks/Ow2/Diamond.webp" },
                new GameRank { Id = 21, GameId = 3, RankName = "Master", RankImageUrl = "/images/ranks/Ow2/Master.webp" },
                new GameRank { Id = 22, GameId = 3, RankName = "Grandmaster", RankImageUrl = "/images/ranks/Ow2/Grandmaster.webp" },

                // LOL Ranks (GameId = 4)
                new GameRank { Id = 23, GameId = 4, RankName = "Unranked", RankImageUrl = "/images/ranks/LoL/Unranked.webp" },
                new GameRank { Id = 24, GameId = 4, RankName = "Iron", RankImageUrl = "/images/ranks/LoL/Iron.webp" },
                new GameRank { Id = 25, GameId = 4, RankName = "Bronze", RankImageUrl = "/images/ranks/LoL/Bronze.webp" },
                new GameRank { Id = 26, GameId = 4, RankName = "Silver", RankImageUrl = "/images/ranks/LoL/Silver.webp" },
                new GameRank { Id = 27, GameId = 4, RankName = "Gold", RankImageUrl = "/images/ranks/LoL/Gold.webp" },
                new GameRank { Id = 28, GameId = 4, RankName = "Platinum", RankImageUrl = "/images/ranks/LoL/Platinum.webp" },
                new GameRank { Id = 29, GameId = 4, RankName = "Emerald", RankImageUrl = "/images/ranks/LoL/Emerald.webp" },
                new GameRank { Id = 30, GameId = 4, RankName = "Diamond", RankImageUrl = "/images/ranks/LoL/Diamond.webp" },
                new GameRank { Id = 31, GameId = 4, RankName = "Master", RankImageUrl = "/images/ranks/LoL/Master.webp" },
                new GameRank { Id = 32, GameId = 4, RankName = "Grandmaster", RankImageUrl = "/images/ranks/LoL/Grandmaster.webp" },
                new GameRank { Id = 33, GameId = 4, RankName = "Challenger", RankImageUrl = "/images/ranks/LoL/Challenger.webp" },
                
                // PUBG Ranks (GameId = 9)
                new GameRank { Id = 34, GameId = 9, RankName = "Bronze", RankImageUrl = "/images/ranks/Pubg/Bronze.webp" },
                new GameRank { Id = 35, GameId = 9, RankName = "Silver", RankImageUrl = "/images/ranks/Pubg/Silver.webp" },
                new GameRank { Id = 36, GameId = 9, RankName = "Gold", RankImageUrl = "/images/ranks/Pubg/Gold.webp" },
                new GameRank { Id = 37, GameId = 9, RankName = "Platinum", RankImageUrl = "/images/ranks/Pubg/Platinum.webp" },
                new GameRank { Id = 38, GameId = 9, RankName = "Crown", RankImageUrl = "/images/ranks/Pubg/Crown.webp" },
                new GameRank { Id = 39, GameId = 9, RankName = "Ace", RankImageUrl = "/images/ranks/Pubg/Ace.webp" },
                new GameRank { Id = 40, GameId = 9, RankName = "AceMaster", RankImageUrl = "/images/ranks/Pubg/AceMaster.webp" },
                new GameRank { Id = 41, GameId = 9, RankName = "AceDominator", RankImageUrl = "/images/ranks/Pubg/AceDominator.webp" },
                new GameRank { Id = 42, GameId = 9, RankName = "Conqueror", RankImageUrl = "/images/ranks/Pubg/Conqueror.webp" }
            );
        }
    }
}