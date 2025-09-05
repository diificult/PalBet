using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PalBet.Models;

namespace PalBet.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public DbSet<Bet> bets { get; set; }
        public DbSet<BetParticipant> participants { get; set; }

        public DbSet<Friendship> friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BetParticipant>().HasKey(bp => new { bp.betId, bp.appUserId });

            builder.Entity<Friendship>().HasKey(f => new {f.RequesterId, f.RequesteeId});

            builder.Entity<Friendship>()
                .HasOne(f => f.Requester)
                .WithMany(i => i.friendshipsInstantiated)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Friendship>()
                .HasOne(f => f.Requestee)
                .WithMany(r => r.friendshipsRecieved)
                .HasForeignKey(f => f.RequesteeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BetParticipant>()
                .HasOne(bp => bp.bet)
                .WithMany(b => b.Participants)
                .HasForeignKey(bp => bp.betId);
            builder.Entity<BetParticipant>()
                .HasOne(bp => bp.appUser)
                .WithMany(u => u.BetsParticipation)
                .HasForeignKey(bp => bp.appUserId);
            builder.Entity<IdentityRole>().HasData(


              new IdentityRole
              {
                  Id = "1",
                  Name = "Admin",
                  NormalizedName = "ADMIN",
              },
              new IdentityRole
              {
                  Id = "2",
                  Name = "User",
                  NormalizedName = "USER",
              }
          );
        }
    }
}
