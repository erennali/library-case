using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Notification
{
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    public NotificationType Type { get; set; }

    public NotificationStatus Status { get; set; } = NotificationStatus.Unread;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }

    public int? RelatedEntityId { get; set; }

    [StringLength(50)]
    public string? RelatedEntityType { get; set; }

    public bool IsEmailSent { get; set; } = false;

    public DateTime? EmailSentAt { get; set; }
}

