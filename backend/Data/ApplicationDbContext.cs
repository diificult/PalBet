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

        public DbSet<Notification> notification { get; set; }   

        public DbSet<Group> Groups { get; set; }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BetParticipant>().HasKey(bp => new { bp.BetId, bp.AppUserId });

            builder.Entity<Friendship>().HasKey(f => new {f.RequesterId, f.RequesteeId});
            
            builder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });

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
                .HasOne(bp => bp.Bet)
                .WithMany(b => b.Participants)
                .HasForeignKey(bp => bp.BetId);
            builder.Entity<BetParticipant>()
                .HasOne(bp => bp.AppUser)
                .WithMany(u => u.BetsParticipation)
                .HasForeignKey(bp => bp.AppUserId);

            builder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            builder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            builder.Entity<Group>()
                .HasMany(g => g.GroupBets)
                .WithOne(b => b.Group)
                .HasForeignKey(b => b.GroupId);


            builder.Entity<BetChoice>()
                .HasOne(c => c.Bet)
                .WithMany(b => b.Choices)
                .HasForeignKey(c => c.BetId);




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
