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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BetParticipant>().HasKey(bp => new { bp.betId, bp.appUserId });

            builder.Entity<BetParticipant>()
                .HasOne(bp => bp.bet)
                .WithMany(b => b.Participants)
                .HasForeignKey(bp => bp.betId);
            builder.Entity<BetParticipant>()
                .HasOne(bp => bp.appUser)
                .WithMany(u => u.BetsParticipation)
                .HasForeignKey(bp => bp.appUserId);

        }
    }
}
