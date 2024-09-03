// Data/ApplicationDbContext.cs
using BreweryManagementSystemWebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace BreweryManagementSystemWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Wholesaler> Wholesalers { get; set; }
        public DbSet<WholesalerStock> WholesalerStocks { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Beer entity
            modelBuilder.Entity<Beer>()
                .ToTable("Beer")
                .HasKey(b => b.be_Id);

            modelBuilder.Entity<Beer>()
                .Property(b => b.be_Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Beer>()
                .Property(b => b.be_Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Beer>()
                .HasOne<Brewery>(b => b.Brewery)
                .WithMany(br => br.Beers)
                .HasForeignKey(b => b.br_Id);

            // Configure Brewery entity
            modelBuilder.Entity<Brewery>()
                .ToTable("Brewery")
                .HasKey(br => br.br_Id);

            modelBuilder.Entity<Brewery>()
                .Property(br => br.br_Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Brewery>()
                .Property(br => br.br_Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure Wholesaler entity
            modelBuilder.Entity<Wholesaler>()
                .ToTable("Wholesaler")
                .HasKey(w => w.wh_Id);

            modelBuilder.Entity<Wholesaler>()
                .Property(w => w.wh_Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Wholesaler>()
                .Property(w => w.wh_Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure WholesalerStock entity
            modelBuilder.Entity<WholesalerStock>()
                .ToTable("WholesalerStock")
                .HasKey(ws => ws.wh_Id);

            modelBuilder.Entity<WholesalerStock>()
                .Property(ws => ws.wh_Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<WholesalerStock>()
                .Property(ws => ws.ws_Quantity)
                .IsRequired();

            modelBuilder.Entity<WholesalerStock>()
                .HasOne<Wholesaler>(ws => ws.Wholesaler)
                .WithMany(w => w.WholesalerStocks)
                .HasForeignKey(ws => ws.wh_Id);

            modelBuilder.Entity<WholesalerStock>()
                .HasOne<Beer>(ws => ws.Beer)
                .WithMany(b => b.WholesalerStocks)
                .HasForeignKey(ws => ws.be_Id);


            modelBuilder.Entity<Quote>()
             .ToTable("Quote")
             .HasNoKey();
        }
    }
}
