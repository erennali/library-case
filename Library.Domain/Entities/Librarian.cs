using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Librarian
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public LibrarianRole Role { get; set; } = LibrarianRole.Assistant;

    public LibrarianStatus Status { get; set; } = LibrarianStatus.Active;

    public DateTime HireDate { get; set; }

    [StringLength(100)]
    public string? Department { get; set; }

    [StringLength(500)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Transaction> ProcessedTransactions { get; set; } = new List<Transaction>();
    public ICollection<Reservation> ProcessedReservations { get; set; } = new List<Reservation>();
    public ICollection<Fine> ProcessedFines { get; set; } = new List<Fine>();
}

