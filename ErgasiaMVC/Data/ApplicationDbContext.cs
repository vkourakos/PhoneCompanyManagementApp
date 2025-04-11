using ErgasiaMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErgasiaMVC.Data;
public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Models.Program> Programs { get; set; }
    public DbSet<Call> Calls { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Bill
        modelBuilder.Entity<Bill>()
            .HasMany(b => b.Calls)
            .WithMany(c => c.Bills)
            .UsingEntity(j => j.ToTable("BillsCalls"));

        modelBuilder.Entity<Bill>()
            .Property(c => c.PhoneNumber)
           .HasMaxLength(15);

        modelBuilder.Entity<Bill>()
           .Property(b => b.Costs)
           .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Bill>()
            .HasOne(b => b.Phone)
            .WithMany()
            .HasForeignKey(b => b.PhoneNumber)
            .OnDelete(DeleteBehavior.Restrict);

        //Program
        modelBuilder.Entity<Models.Program>()
            .Property(p => p.Charge)
            .HasColumnType("decimal(18,2)");

        //Admin
        modelBuilder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Admin>()
            .HasIndex(a => a.UserId).IsUnique();

        //Seller
        modelBuilder.Entity<Seller>()
            .HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Seller>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Seller>()
            .HasIndex(s => s.UserId).IsUnique();

        //Client
        modelBuilder.Entity<Client>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Client>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Client>()
            .Property(c => c.Afm)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Client>()
           .Property(c => c.PhoneNumber)
           .IsRequired()
           .HasMaxLength(15);

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Afm).IsUnique();

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.UserId).IsUnique();

        modelBuilder.Entity<Client>()
            .HasOne(c => c.Phone)
            .WithMany()
            .HasForeignKey(c => c.PhoneNumber)
            .OnDelete(DeleteBehavior.Restrict);

        //Phone
        modelBuilder.Entity<Phone>()
            .HasOne(p => p.Program)
            .WithMany()
            .HasForeignKey(p => p.ProgramName);

        modelBuilder.Entity<Phone>()
            .Property(c => c.PhoneNumber)
            .HasMaxLength(15);

        //User
        modelBuilder.Entity<User>()
            .Property(u => u.FirstName)
           .IsRequired()
           .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.Property)
            .HasMaxLength(50);

        //Call
        modelBuilder.Entity<Call>()
            .HasOne(c => c.Caller)
            .WithMany()
            .HasForeignKey(c => c.CallerNumber)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Call>()
            .HasOne(c => c.Callee)
            .WithMany()
            .HasForeignKey(c => c.CalleeNumber)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Call>()
           .Property(c => c.Description)
           .HasMaxLength(50);
    }
}
