using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public int? UserId { get; set; }
    public string? UserEmail { get; set; }
    public string UserType { get; set; } = string.Empty; // Member, Librarian
    public string EntityType { get; set; } = string.Empty; // Book, Member, Transaction, etc.
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
