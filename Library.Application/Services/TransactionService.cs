using Library.Application.Abstractions.Repositories;
using Library.Application.Abstractions.Services;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionResponseDto> BorrowBookAsync(BorrowBookRequest request, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, ct);
        if (book == null)
            throw new KeyNotFoundException($"Book {request.BookId} not found");

        var member = await _memberRepository.GetByIdAsync(request.MemberId, ct);
        if (member == null)
            throw new KeyNotFoundException($"Member {request.MemberId} not found");

        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("Book is not available");

        if (member.CurrentBooksCount >= member.MaxBooksAllowed)
            throw new InvalidOperationException("Member has reached maximum book limit");

        var transaction = new Transaction
        {
            BookId = request.BookId,
            MemberId = request.MemberId,
            Type = TransactionType.Checkout,
            CheckoutDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(request.Days),
            Status = TransactionStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Notes = request.Notes
        };

        var createdTransaction = await _transactionRepository.CreateAsync(transaction, ct);

        // Update book availability
        book.AvailableCopies--;
        _bookRepository.Update(book);

        // Update member book count
        member.CurrentBooksCount++;
        await _memberRepository.UpdateAsync(member, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        return await GetByIdAsync(createdTransaction.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve created transaction");
    }

    public async Task<TransactionResponseDto> ReturnBookAsync(ReturnBookRequest request, CancellationToken ct = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, ct);
        if (transaction == null)
            throw new KeyNotFoundException($"Transaction {request.TransactionId} not found");

        if (transaction.Status == TransactionStatus.Returned)
            throw new InvalidOperationException("Book is already returned");

        transaction.ReturnDate = DateTime.UtcNow;
        transaction.Status = TransactionStatus.Returned;
        transaction.UpdatedAt = DateTime.UtcNow;
        transaction.Notes = request.Notes;

        await _transactionRepository.UpdateAsync(transaction, ct);

        // Update book availability
        var book = await _bookRepository.GetByIdAsync(transaction.BookId, ct);
        if (book != null)
        {
            book.AvailableCopies++;
            _bookRepository.Update(book);
        }

        // Update member book count
        var member = await _memberRepository.GetByIdAsync(transaction.MemberId, ct);
        if (member != null && member.CurrentBooksCount > 0)
        {
            member.CurrentBooksCount--;
            await _memberRepository.UpdateAsync(member, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return await GetByIdAsync(transaction.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve updated transaction");
    }

    public async Task<TransactionResponseDto> RenewBookAsync(RenewBookRequest request, CancellationToken ct = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, ct);
        if (transaction == null)
            throw new KeyNotFoundException($"Transaction {request.TransactionId} not found");

        if (transaction.Status != TransactionStatus.Active)
            throw new InvalidOperationException("Transaction is not active");

        transaction.DueDate = transaction.DueDate.AddDays(request.AdditionalDays);
        transaction.UpdatedAt = DateTime.UtcNow;
        transaction.Notes = request.Notes;

        await _transactionRepository.UpdateAsync(transaction, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return await GetByIdAsync(transaction.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve updated transaction");
    }

    public async Task<TransactionResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, ct);
        if (transaction == null)
            return null;

        return new TransactionResponseDto(
            transaction.Id,
            transaction.TransactionNumber,
            transaction.BookId,
            transaction.Book.Title,
            transaction.MemberId,
            $"{transaction.Member.FirstName} {transaction.Member.LastName}",
            transaction.Type,
            transaction.CheckoutDate,
            transaction.DueDate,
            transaction.ReturnDate,
            transaction.Status,
            transaction.RenewalCount,
            transaction.FineAmount,
            transaction.Notes,
            transaction.ProcessedByLibrarianId,
            transaction.ProcessedByLibrarian != null ? $"{transaction.ProcessedByLibrarian.FirstName} {transaction.ProcessedByLibrarian.LastName}" : null,
            transaction.CreatedAt
        );
    }

    public async Task<PagedResult<TransactionResponseDto>> GetByMemberAsync(int memberId, int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _transactionRepository.GetByMemberAsync(memberId, page, pageSize, ct);
        var items = result.Items.Select(t => new TransactionResponseDto(
            t.Id,
            t.TransactionNumber,
            t.BookId,
            t.Book.Title,
            t.MemberId,
            $"{t.Member.FirstName} {t.Member.LastName}",
            t.Type,
            t.CheckoutDate,
            t.DueDate,
            t.ReturnDate,
            t.Status,
            t.RenewalCount,
            t.FineAmount,
            t.Notes,
            t.ProcessedByLibrarianId,
            t.ProcessedByLibrarian != null ? $"{t.ProcessedByLibrarian.FirstName} {t.ProcessedByLibrarian.LastName}" : null,
            t.CreatedAt
        )).ToList();

        return new PagedResult<TransactionResponseDto>(items, result.TotalCount, page, pageSize);
    }

    public async Task<PagedResult<TransactionResponseDto>> GetByBookAsync(int bookId, int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _transactionRepository.GetByBookAsync(bookId, page, pageSize, ct);
        var items = result.Items.Select(t => new TransactionResponseDto(
            t.Id,
            t.TransactionNumber,
            t.BookId,
            t.Book.Title,
            t.MemberId,
            $"{t.Member.FirstName} {t.Member.LastName}",
            t.Type,
            t.CheckoutDate,
            t.DueDate,
            t.ReturnDate,
            t.Status,
            t.RenewalCount,
            t.FineAmount,
            t.Notes,
            t.ProcessedByLibrarianId,
            t.ProcessedByLibrarian != null ? $"{t.ProcessedByLibrarian.FirstName} {t.ProcessedByLibrarian.LastName}" : null,
            t.CreatedAt
        )).ToList();

        return new PagedResult<TransactionResponseDto>(items, result.TotalCount, page, pageSize);
    }

    public async Task<PagedResult<TransactionResponseDto>> GetOverdueAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _transactionRepository.GetOverdueAsync(page, pageSize, ct);

        var items = result.Items.Select(t => new TransactionResponseDto(
            t.Id,
            t.TransactionNumber,
            t.BookId,
            t.Book.Title,
            t.MemberId,
            $"{t.Member.FirstName} {t.Member.LastName}",
            t.Type,
            t.CheckoutDate,
            t.DueDate,
            t.ReturnDate,
            t.Status,
            t.RenewalCount,
            t.FineAmount,
            t.Notes,
            t.ProcessedByLibrarianId,
            t.ProcessedByLibrarian != null ? $"{t.ProcessedByLibrarian.FirstName} {t.ProcessedByLibrarian.LastName}" : null,
            t.CreatedAt
        )).ToList();

        return new PagedResult<TransactionResponseDto>(items, result.TotalCount, page, pageSize);
    }

    public async Task<PagedResult<TransactionResponseDto>> GetActiveAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var result = await _transactionRepository.GetActiveAsync(page, pageSize, ct);

        var items = result.Items.Select(t => new TransactionResponseDto(
            t.Id,
            t.TransactionNumber,
            t.BookId,
            t.Book.Title,
            t.MemberId,
            $"{t.Member.FirstName} {t.Member.LastName}",
            t.Type,
            t.CheckoutDate,
            t.DueDate,
            t.ReturnDate,
            t.Status,
            t.RenewalCount,
            t.FineAmount,
            t.Notes,
            t.ProcessedByLibrarianId,
            t.ProcessedByLibrarian != null ? $"{t.ProcessedByLibrarian.FirstName} {t.ProcessedByLibrarian.LastName}" : null,
            t.CreatedAt
        )).ToList();

        return new PagedResult<TransactionResponseDto>(items, result.TotalCount, page, pageSize);
    }
}
