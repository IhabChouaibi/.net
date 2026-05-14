using LivreurService.Models;
using Microsoft.EntityFrameworkCore;

namespace LivreurService.Data;

public class LivreurDbContext : DbContext
{
    public LivreurDbContext(DbContextOptions<LivreurDbContext> options)
        : base(options)
    {
    }

    public DbSet<Livreur> Livreurs => Set<Livreur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Livreur>(entity =>
        {
            entity.ToTable("Livreurs");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CIN).HasMaxLength(20).IsRequired();
            entity.Property(x => x.RaisonSocial).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Ville).HasMaxLength(80).IsRequired();
            entity.Property(x => x.CodePostal).HasMaxLength(10).IsRequired();
            entity.HasIndex(x => x.CIN).IsUnique();

            entity.HasData(
                new Livreur { Id = 1, CIN = "12345678", RaisonSocial = "Express Nord", Ville = "Tunis", CodePostal = "1000" },
                new Livreur { Id = 2, CIN = "87654321", RaisonSocial = "Rapid Sud", Ville = "Sfax", CodePostal = "3000" },
                new Livreur { Id = 3, CIN = "11223344", RaisonSocial = "Livraison Centre", Ville = "Sousse", CodePostal = "4000" });
        });
    }
}
