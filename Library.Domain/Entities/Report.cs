using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class Report
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string ReportType { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Format { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [StringLength(500)]
    public string? FilePath { get; set; }

    [StringLength(500)]
    public string? FileUrl { get; set; }

    public long? FileSize { get; set; }

    public string? Parameters { get; set; } // JSON

    public DateTime? GeneratedAt { get; set; }

    public DateTime? ScheduledAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? CreatedByLibrarianId { get; set; }

    public Librarian? CreatedByLibrarian { get; set; }
}



