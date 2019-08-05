using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PogGames.Model
{
    public partial class PogGamesContext : DbContext
    {
        public PogGamesContext()
        {
        }

        public PogGamesContext(DbContextOptions<PogGamesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Character> Character { get; set; }
        public virtual DbSet<Game> Game { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:poggamesmsa.database.windows.net,1433;Initial Catalog=PogGames;Persist Security Info=False;User ID=frankwangma;Password=Loquito123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.CharId)
                    .HasName("PK__Characte__AA7BC2746DC2D953");

                entity.Property(e => e.CharId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CharCountry).IsUnicode(false);

                entity.Property(e => e.CharDescription).IsUnicode(false);

                entity.Property(e => e.CharGender).IsUnicode(false);

                entity.Property(e => e.CharImageUrl).IsUnicode(false);

                entity.Property(e => e.CharName).IsUnicode(false);

                entity.Property(e => e.GameId).IsUnicode(false);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Character)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("GameId");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CoverImageUrl).IsUnicode(false);

                entity.Property(e => e.GameCompany).IsUnicode(false);

                entity.Property(e => e.GameName).IsUnicode(false);

                entity.Property(e => e.GameRelease).IsUnicode(false);

                entity.Property(e => e.GameSummary).IsUnicode(false);

                entity.Property(e => e.Genre).IsUnicode(false);

                entity.Property(e => e.Rating).IsUnicode(false);
            });
        }
    }
}
