using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class LibrarySettings
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Value { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? UpdatedByLibrarianId { get; set; }
    public Librarian? UpdatedByLibrarian { get; set; }
}