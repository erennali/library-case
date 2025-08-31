namespace Library.Application.DTOs;

public record CreateReviewRequest(
    int BookId,
    int MemberId,
    int Rating,
    string? Comment = null
);

public record UpdateReviewRequest(
    int Rating,
    string? Comment = null
);

public record RejectReviewRequest(
    string Reason,
    string? Notes = null
);

public record ReviewResponseDto(
    int Id,
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    int Rating,
    string? Comment,
    DateTime ReviewDate,
    bool IsApproved
);

public record BookReviewStatsDto(
    int BookId,
    string BookTitle,
    int TotalReviews,
    int ApprovedReviews,
    int PendingReviews,
    double AverageRating,
    int OneStarCount,
    int TwoStarCount,
    int ThreeStarCount,
    int FourStarCount,
    int FiveStarCount
);

public record ApproveReviewRequest(
    string? Notes = null
);
