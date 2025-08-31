using Library.Application.Abstractions.Repositories;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class FineService : IFineService
{
    private readonly IFineRepository _fineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FineService(IFineRepository fineRepository, IUnitOfWork unitOfWork)
    {
        _fineRepository = fineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FineResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var fine = await _fineRepository.GetByIdAsync(id, ct);
        if (fine == null)
            return null;

        return new FineResponseDto(
            fine.Id,
            fine.FineNumber,
            fine.TransactionId,
            fine.Transaction.TransactionNumber,
            fine.MemberId,
            $"{fine.Member.FirstName} {fine.Member.LastName}",
            fine.Type,
            fine.Amount,
            fine.IssueDate,
            fine.DueDate,
            fine.Status,
            fine.PaidDate,
            fine.PaidAmount,
            fine.Description,
            fine.Notes,
            fine.ProcessedByLibrarianId,
            fine.ProcessedByLibrarian != null ? $"{fine.ProcessedByLibrarian.FirstName} {fine.ProcessedByLibrarian.LastName}" : null,
            fine.CreatedAt,
            fine.UpdatedAt
        );
    }

    public async Task<PagedResult<FineResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _fineRepository.GetByMemberAsync(memberId, page, pageSize, ct);
        var items = result.Items.Select(f => new FineResponseDto(
            f.Id,
            f.FineNumber,
            f.TransactionId,
            f.Transaction.TransactionNumber,
            f.MemberId,
            $"{f.Member.FirstName} {f.Member.LastName}",
            f.Type,
            f.Amount,
            f.IssueDate,
            f.DueDate,
            f.Status,
            f.PaidDate,
            f.PaidAmount,
            f.Description,
            f.Notes,
            f.ProcessedByLibrarianId,
            f.ProcessedByLibrarian != null ? $"{f.ProcessedByLibrarian.FirstName} {f.ProcessedByLibrarian.LastName}" : null,
            f.CreatedAt,
            f.UpdatedAt
        )).ToList();

        return new PagedResult<FineResponseDto>(items, result.TotalCount, page, pageSize);
    }

    public async Task<PagedResult<FineResponseDto>> GetPendingAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _fineRepository.GetPendingAsync(page, pageSize, ct);
        var items = result.Items.Select(f => new FineResponseDto(
            f.Id,
            f.FineNumber,
            f.TransactionId,
            f.Transaction.TransactionNumber,
            f.MemberId,
            $"{f.Member.FirstName} {f.Member.LastName}",
            f.Type,
            f.Amount,
            f.IssueDate,
            f.DueDate,
            f.Status,
            f.PaidDate,
            f.PaidAmount,
            f.Description,
            f.Notes,
            f.ProcessedByLibrarianId,
            f.ProcessedByLibrarian != null ? $"{f.ProcessedByLibrarian.FirstName} {f.ProcessedByLibrarian.LastName}" : null,
            f.CreatedAt,
            f.UpdatedAt
        )).ToList();

        return new PagedResult<FineResponseDto>(items, result.TotalCount, page, pageSize);
    }

    public async Task<PagedResult<FineResponseDto>> GetOverdueAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _fineRepository.GetOverdueAsync(page, pageSize, ct);
        var items = result.Items.Select(f => new FineResponseDto(
            f.Id,
            f.FineNumber,
            f.TransactionId,
            f.Transaction.TransactionNumber,
            f.MemberId,
            $"{f.Member.FirstName} {f.Member.LastName}",
            f.Type,
            f.Amount,
            f.IssueDate,
            f.DueDate,
            f.Status,
            f.PaidDate,
            f.PaidAmount,
            f.Description,
            f.Notes,
            f.ProcessedByLibrarianId,
            f.ProcessedByLibrarian != null ? $"{f.ProcessedByLibrarian.FirstName} {f.ProcessedByLibrarian.LastName}" : null,
            f.CreatedAt,
            f.UpdatedAt
        )).ToList();

        return new PagedResult<FineResponseDto>(items, result.TotalCount, page, pageSize);
    }

    public async Task<FineResponseDto> PayFineAsync(PayFineRequest request, CancellationToken ct = default)
    {
        var fine = await _fineRepository.GetByIdAsync(request.FineId, ct);
        if (fine == null)
            throw new KeyNotFoundException($"Fine {request.FineId} not found");

        if (fine.Status != Domain.Enums.FineStatus.Pending && fine.Status != Domain.Enums.FineStatus.Unpaid)
            throw new InvalidOperationException("Fine is not payable");

        fine.PaidAmount = request.Amount;
        fine.PaidDate = DateTime.UtcNow;
        fine.Status = request.Amount >= fine.Amount ? Domain.Enums.FineStatus.Paid : Domain.Enums.FineStatus.PartiallyPaid;
        fine.Notes = request.Notes;
        fine.UpdatedAt = DateTime.UtcNow;

        await _fineRepository.UpdateAsync(fine, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return await GetByIdAsync(fine.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve updated fine");
    }

    public async Task<FineResponseDto> WaiveFineAsync(WaiveFineRequest request, CancellationToken ct = default)
    {
        var fine = await _fineRepository.GetByIdAsync(request.FineId, ct);
        if (fine == null)
            throw new KeyNotFoundException($"Fine {request.FineId} not found");

        fine.Status = Domain.Enums.FineStatus.Waived;
        fine.Notes = request.Notes;
        fine.UpdatedAt = DateTime.UtcNow;

        await _fineRepository.UpdateAsync(fine, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return await GetByIdAsync(fine.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve updated fine");
    }

    public async Task<FineSummaryDto> GetMemberSummaryAsync(int memberId, CancellationToken ct = default)
    {
        var fines = await _fineRepository.GetByMemberAsync(memberId, 1, int.MaxValue, ct);
        var member = fines.Items.FirstOrDefault()?.Member;

        var totalFines = fines.Items.Count;
        var pendingFines = fines.Items.Count(f => f.Status == Domain.Enums.FineStatus.Pending || f.Status == Domain.Enums.FineStatus.Unpaid);
        var paidFines = fines.Items.Count(f => f.Status == Domain.Enums.FineStatus.Paid || f.Status == Domain.Enums.FineStatus.PartiallyPaid);

        var totalAmount = fines.Items.Sum(f => f.Amount);
        var pendingAmount = fines.Items.Where(f => f.Status == Domain.Enums.FineStatus.Pending || f.Status == Domain.Enums.FineStatus.Unpaid).Sum(f => f.Amount);
        var paidAmount = fines.Items.Where(f => f.Status == Domain.Enums.FineStatus.Paid || f.Status == Domain.Enums.FineStatus.PartiallyPaid).Sum(f => f.PaidAmount ?? 0);

        return new FineSummaryDto(
            memberId,
            member != null ? $"{member.FirstName} {member.LastName}" : "Unknown",
            totalFines,
            pendingFines,
            paidFines,
            totalAmount,
            pendingAmount,
            paidAmount
        );
    }

    public async Task<OverallFineSummaryDto> GetOverallSummaryAsync(CancellationToken ct = default)
    {
        var pendingResult = await _fineRepository.GetPendingAsync(1, int.MaxValue, ct);
        var overdueResult = await _fineRepository.GetOverdueAsync(1, int.MaxValue, ct);
        var allFines = pendingResult.Items.Concat(overdueResult.Items).Concat(
            (await _fineRepository.GetByMemberAsync(0, 1, int.MaxValue, ct)).Items.Where(f => f.Status == Domain.Enums.FineStatus.Paid || f.Status == Domain.Enums.FineStatus.PartiallyPaid)
        ).Distinct();

        var totalFines = allFines.Count();
        var pendingFines = pendingResult.TotalCount;
        var paidFines = allFines.Count(f => f.Status == Domain.Enums.FineStatus.Paid || f.Status == Domain.Enums.FineStatus.PartiallyPaid);

        var totalAmount = allFines.Sum(f => f.Amount);
        var pendingAmount = pendingResult.Items.Sum(f => f.Amount);
        var paidAmount = allFines.Where(f => f.Status == Domain.Enums.FineStatus.Paid || f.Status == Domain.Enums.FineStatus.PartiallyPaid).Sum(f => f.PaidAmount ?? 0);
        var averageFineAmount = totalFines > 0 ? totalAmount / totalFines : 0;

        return new OverallFineSummaryDto(
            totalFines,
            pendingFines,
            paidFines,
            totalAmount,
            pendingAmount,
            paidAmount,
            averageFineAmount
        );
    }
}
