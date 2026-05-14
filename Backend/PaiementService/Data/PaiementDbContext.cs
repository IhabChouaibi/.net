using Microsoft.EntityFrameworkCore;
using PaiementService.Models;

namespace PaiementService.Data;

public class PaiementDbContext : DbContext
{
    public PaiementDbContext(DbContextOptions<PaiementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Paiement> Paiements => Set<Paiement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paiement>(entity =>
        {
            entity.ToTable("Paiements");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Montant).HasColumnType("decimal(18,2)");
            entity.Property(x => x.ModePaiement).HasMaxLength(30).IsRequired();

            entity.HasData(
                new Paiement { Id = 1, Montant = 120.50m, DatePaiement = new DateTime(2026, 1, 10), ModePaiement = "Carte", ColisId = 1 },
                new Paiement { Id = 2, Montant = 89.90m, DatePaiement = new DateTime(2026, 2, 15), ModePaiement = "Especes", ColisId = 2 },
                new Paiement { Id = 3, Montant = 230m, DatePaiement = new DateTime(2026, 3, 21), ModePaiement = "Virement", ColisId = 3 });
        });
    }
}
