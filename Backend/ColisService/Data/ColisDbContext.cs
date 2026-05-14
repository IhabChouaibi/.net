using ColisService.Models;
using Microsoft.EntityFrameworkCore;

namespace ColisService.Data;

public class ColisDbContext : DbContext
{
    public ColisDbContext(DbContextOptions<ColisDbContext> options)
        : base(options)
    {
    }

    public DbSet<Colis> Colis => Set<Colis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Colis>(entity =>
        {
            entity.ToTable("Colis");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Libelle).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Montant).HasColumnType("decimal(18,2)");

            entity.HasData(
                new Colis
                {
                    Id = 1,
                    Libelle = "Ordinateur portable",
                    DateLivraison = new DateTime(2026, 1, 12),
                    Montant = 2200m,
                    Poids = 2.5,
                    Volume = 0.01,
                    ClientId = 1,
                    LivreurId = 1,
                    StatutLivraisonId = 3
                },
                new Colis
                {
                    Id = 2,
                    Libelle = "Documents administratifs",
                    DateLivraison = new DateTime(2026, 2, 18),
                    Montant = 35m,
                    Poids = 0.2,
                    Volume = 0.002,
                    ClientId = 2,
                    LivreurId = 2,
                    StatutLivraisonId = 1
                },
                new Colis
                {
                    Id = 3,
                    Libelle = "Pieces auto",
                    DateLivraison = new DateTime(2026, 3, 25),
                    Montant = 780m,
                    Poids = 12.3,
                    Volume = 0.15,
                    ClientId = 3,
                    LivreurId = 3,
                    StatutLivraisonId = 2
                });
        });
    }
}
