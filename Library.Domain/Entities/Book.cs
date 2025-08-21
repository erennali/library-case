using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(13)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Author { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Publisher { get; set; }

    public DateTime? PublicationDate { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    [StringLength(50)]
    public string? Language { get; set; }

    public int PageCount { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public decimal? Price { get; set; }

    public BookStatus Status { get; set; } = BookStatus.Available;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

