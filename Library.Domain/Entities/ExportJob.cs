using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class ExportJob
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string ExportType { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Format { get; set; } = string.Empty;

    public string? Filters { get; set; } // JSON

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int ProcessedRecords { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    [StringLength(500)]
    public string DownloadUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? CreatedByLibrarianId { get; set; }

    public Librarian? CreatedByLibrarian { get; set; }
}



