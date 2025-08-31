namespace Library.Application.DTOs;

public record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);

public record FileResultDto(
    byte[] FileBytes,
    string ContentType,
    string FileName,
    long FileSize
);

public record ErrorResponseDto(
    string Message,
    string? Details = null,
    string? CorrelationId = null,
    DateTime Timestamp = default
);

public record SuccessResponseDto(
    string Message,
    object? Data = null,
    DateTime Timestamp = default
);

public record PaginationRequest(
    int Page = 1,
    int PageSize = 20
);

public record DateRangeRequest(
    DateTime? FromDate = null,
    DateTime? ToDate = null
);

public record SortRequest(
    string SortBy = "Id",
    string SortDirection = "asc"
);

public record FilterRequest(
    Dictionary<string, object> Filters
);

public record BulkOperationRequest<T>(
    List<int> Ids,
    T Data
);

public record BulkOperationResult(
    int TotalCount,
    int SuccessCount,
    int FailureCount,
    List<string> Errors
);

public record IdRequest(
    int Id
);

public record IdsRequest(
    List<int> Ids
);

public record NameValueDto(
    string Name,
    string Value
);

public record KeyValueDto<TKey, TValue>(
    TKey Key,
    TValue Value
);

public record SelectOptionDto(
    string Value,
    string Text,
    string? Group = null,
    bool IsDisabled = false
);

public record LookupDto(
    int Id,
    string Name,
    string? Description = null,
    bool IsActive = true
);

public record StatusDto(
    string Status,
    string Message,
    DateTime Timestamp = default
);

public record CountDto(
    int Count
);

public record TotalDto(
    decimal Total
);

public record AverageDto(
    double Average
);

public record MinMaxDto<T>(
    T Min,
    T Max
);

public record RangeDto<T>(
    T From,
    T To
);

public record PercentageDto(
    double Value,
    double Total,
    double Percentage
);

public record GrowthDto(
    double CurrentValue,
    double PreviousValue,
    double Growth,
    double GrowthPercentage
);

public record TrendDataDto(
    DateTime Date,
    Dictionary<string, decimal> Values
);

public record DailyCirculationDto(
    DateTime Date,
    int Checkouts,
    int Returns,
    int Renewals,
    int Overdue
);
