using LivraisonService.Models;
using Microsoft.EntityFrameworkCore;

namespace LivraisonService.Data;

public class LivraisonDbContext : DbContext
{
    public LivraisonDbContext(DbContextOptions<LivraisonDbContext> options)
        : base(options)
    {
    }

    public DbSet<AdresseLivraison> AdressesLivraison => Set<AdresseLivraison>();
    public DbSet<StatutLivraison> StatutsLivraison => Set<StatutLivraison>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatutLivraison>(entity =>
        {
            entity.ToTable("StatutsLivraison");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Libelle).HasMaxLength(50).IsRequired();

            entity.HasData(
                new StatutLivraison { Id = 1, Libelle = "En attente" },
                new StatutLivraison { Id = 2, Libelle = "En cours" },
                new StatutLivraison { Id = 3, Libelle = "Livre" },
                new StatutLivraison { Id = 4, Libelle = "Annule" });
        });

        modelBuilder.Entity<AdresseLivraison>(entity =>
        {
            entity.ToTable("AdressesLivraison");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Adresse).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Ville).HasMaxLength(80).IsRequired();
            entity.Property(x => x.CodePostal).HasMaxLength(10).IsRequired();

            entity.HasData(
                new AdresseLivraison { Id = 1, Adresse = "10 Rue de Marseille", Ville = "Tunis", CodePostal = "1000", ColisId = 1 },
                new AdresseLivraison { Id = 2, Adresse = "25 Avenue Habib Bourguiba", Ville = "Sfax", CodePostal = "3000", ColisId = 2 },
                new AdresseLivraison { Id = 3, Adresse = "8 Rue des Oliviers", Ville = "Sousse", CodePostal = "4000", ColisId = 3 });
        });
    }
}
