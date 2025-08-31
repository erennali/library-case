using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class ImportJob
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    [Required]
    [StringLength(100)]
    public string ImportType { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int ProcessedRecords { get; set; }

    public int SuccessRecords { get; set; }

    public int FailedRecords { get; set; }

    public string? Errors { get; set; } // JSON array

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? CreatedByLibrarianId { get; set; }

    public Librarian? CreatedByLibrarian { get; set; }
}



