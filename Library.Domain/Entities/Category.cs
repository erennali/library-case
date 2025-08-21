using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation Properties
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Book> Books { get; set; } = new List<Book>();
}