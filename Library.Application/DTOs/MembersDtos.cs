namespace Library.Application.DTOs;

public record ExtendMembershipRequest(
    DateTime NewEndDate,
    string? Reason = null
);
