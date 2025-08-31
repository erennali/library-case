using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string TransactionNumber { get; set; } = string.Empty;

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public TransactionType Type { get; set; }

    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public TransactionStatus Status { get; set; } = TransactionStatus.Active;

    public int? RenewalCount { get; set; } = 0;
    public int MaxRenewalsAllowed { get; set; } = 2;

    public decimal? FineAmount { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? ProcessedByLibrarianId { get; set; }
    public Librarian? ProcessedByLibrarian { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
}

