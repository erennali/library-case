using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Fine
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FineNumber { get; set; } = string.Empty;

    [Required]
    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;

    [Required]
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public FineType Type { get; set; } = FineType.OverdueBook;

    public decimal Amount { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public FineStatus Status { get; set; } = FineStatus.Pending;

    public DateTime? PaidDate { get; set; }

    public decimal? PaidAmount { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? ProcessedByLibrarianId { get; set; }
    public Librarian? ProcessedByLibrarian { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

