using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ReservationNumber { get; set; } = string.Empty;

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

    public DateTime ExpiryDate { get; set; }

    public DateTime? NotifiedDate { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Active;

    public int Priority { get; set; } = 1;

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime? FulfilledDate { get; set; }

    public int? ProcessedByLibrarianId { get; set; }
    public Librarian? ProcessedByLibrarian { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

