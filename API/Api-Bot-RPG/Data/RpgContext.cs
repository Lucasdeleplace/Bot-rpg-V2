using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Api_Bot_RPG.Models;

namespace Api_Bot_RPG.Data
{
    public class RpgContext : DbContext
    {
        public RpgContext(DbContextOptions<RpgContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillTree> SkillTrees { get; set; }
        public DbSet<SkillNode> SkillNodes { get; set; }
        public DbSet<SkillEffect> SkillEffects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des relations
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Characters)
                .WithOne()
                .HasForeignKey("PlayerId");

            modelBuilder.Entity<Character>()
                .HasMany(c => c.Competences)
                .WithOne();

            modelBuilder.Entity<Character>()
                .HasMany(c => c.Inventaire)
                .WithOne();

            modelBuilder.Entity<SkillTree>()
                .HasMany(s => s.Nodes)
                .WithOne();

            modelBuilder.Entity<SkillNode>()
                .HasMany(s => s.Effets)
                .WithOne(e => e.SkillNode)
                .HasForeignKey(e => e.SkillNodeId);

            modelBuilder.Entity<SkillNode>()
                .Property(e => e.PrerequisSkillIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(int.Parse)
                         .ToList(),
                    new ValueComparer<List<int>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()
                    ));

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Character)
                .WithOne(c => c.Equipements)
                .HasForeignKey<Equipment>(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration des relations d'équipement avec NO ACTION
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Arme)
                .WithMany()
                .HasForeignKey(e => e.ArmeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Armure)
                .WithMany()
                .HasForeignKey(e => e.ArmureId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Casque)
                .WithMany()
                .HasForeignKey(e => e.CasqueId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Bottes)
                .WithMany()
                .HasForeignKey(e => e.BottesId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Accessoire)
                .WithMany()
                .HasForeignKey(e => e.AccessoireId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}