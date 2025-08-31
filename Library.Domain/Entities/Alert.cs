using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class Alert
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string AlertType { get; set; } = string.Empty; // System, Security, Maintenance, etc.

    [Required]
    [StringLength(20)]
    public string Severity { get; set; } = string.Empty; // Low, Medium, High, Critical

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    public DateTime? AcknowledgedAt { get; set; }

    public int? AcknowledgedByLibrarianId { get; set; }

    public Librarian? AcknowledgedByLibrarian { get; set; }

    [StringLength(1000)]
    public string? AdditionalData { get; set; } // JSON data for additional context

    public string? Source { get; set; } // Which system component generated this alert

    public int Priority { get; set; } = 1; // 1-5, higher is more important
}

