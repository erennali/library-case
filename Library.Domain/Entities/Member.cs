using System.ComponentModel.DataAnnotations;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

public class Member
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string MembershipNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public MembershipType MembershipType { get; set; } = MembershipType.Regular;

    public DateTime MembershipStartDate { get; set; }
    public DateTime MembershipEndDate { get; set; }

    public MemberStatus Status { get; set; } = MemberStatus.Active;

    public int MaxBooksAllowed { get; set; } = 5;
    public int CurrentBooksCount { get; set; } = 0;

    public decimal TotalFinesOwed { get; set; } = 0;
    public decimal MaxFineLimit { get; set; } = 50;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

