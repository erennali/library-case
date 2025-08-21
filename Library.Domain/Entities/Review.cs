using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    
    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    
    [Required]
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [StringLength(1000)]
    public string? Comment { get; set; }
    
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    
    public bool IsApproved { get; set; } = false;
}