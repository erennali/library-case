using Library.Application.DTOs;

namespace Library.Application.Abstractions.Services;

public interface IReportService
{
    Task<ReportResponseDto> GenerateAsync(GenerateReportRequest request, CancellationToken ct = default);
    Task<ReportResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<FileResultDto?> DownloadAsync(int id, CancellationToken ct = default);
    Task<PagedResult<ReportResponseDto>> GetListAsync(int page, int pageSize, string? reportType, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<List<ReportTypeDto>> GetAvailableTypesAsync(CancellationToken ct = default);
    Task<ReportResponseDto> ScheduleAsync(ScheduleReportRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<OverdueSummaryReportDto> GetOverdueSummaryAsync(CancellationToken ct = default);
    Task<CirculationSummaryReportDto> GetCirculationSummaryAsync(DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<MemberActivityReportDto> GetMemberActivityAsync(int memberId, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
    Task<BookPopularityReportDto> GetBookPopularityAsync(int topCount, DateTime? fromDate, DateTime? toDate, CancellationToken ct = default);
}

