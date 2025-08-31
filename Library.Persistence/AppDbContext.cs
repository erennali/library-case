using Microsoft.EntityFrameworkCore;
using Library.Domain.Entities;

namespace Library.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Fine> Fines { get; set; }
    public DbSet<Librarian> Librarians { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<LibrarySettings> LibrarySettings { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<ImportJob> ImportJobs { get; set; }
    public DbSet<ExportJob> ExportJobs { get; set; }
    public DbSet<ExportTemplate> ExportTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision
        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Member>()
            .Property(m => m.TotalFinesOwed)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Member>()
            .Property(m => m.MaxFineLimit)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.FineAmount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Fine>()
            .Property(f => f.Amount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Fine>()
            .Property(f => f.PaidAmount)
            .HasPrecision(10, 2);

        // Configure unique constraints
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique();

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.MembershipNumber)
            .IsUnique();

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.Email)
            .IsUnique();

        modelBuilder.Entity<Librarian>()
            .HasIndex(l => l.EmployeeNumber)
            .IsUnique();

        modelBuilder.Entity<Librarian>()
            .HasIndex(l => l.Email)
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.TransactionNumber)
            .IsUnique();

        modelBuilder.Entity<Reservation>()
            .HasIndex(r => r.ReservationNumber)
            .IsUnique();

        modelBuilder.Entity<Fine>()
            .HasIndex(f => f.FineNumber)
            .IsUnique();

        modelBuilder.Entity<LibrarySettings>()
            .HasIndex(s => s.Key)
            .IsUnique();

        // Configure relationships
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Avoid multiple cascade paths via Member -> Transactions -> Fines and Member -> Fines
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Member)
            .WithMany(m => m.Transactions)
            .HasForeignKey(t => t.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Fine>()
            .HasOne(f => f.Member)
            .WithMany(m => m.Fines)
            .HasForeignKey(f => f.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ProcessedByLibrarian)
            .WithMany(l => l.ProcessedTransactions)
            .HasForeignKey(t => t.ProcessedByLibrarianId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.ProcessedByLibrarian)
            .WithMany(l => l.ProcessedReservations)
            .HasForeignKey(r => r.ProcessedByLibrarianId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Fine>()
            .HasOne(f => f.ProcessedByLibrarian)
            .WithMany(l => l.ProcessedFines)
            .HasForeignKey(f => f.ProcessedByLibrarianId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LibrarySettings>()
            .HasOne(s => s.UpdatedByLibrarian)
            .WithMany()
            .HasForeignKey(s => s.UpdatedByLibrarianId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure composite unique constraints
        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.BookId, r.MemberId })
            .IsUnique();
    }
}