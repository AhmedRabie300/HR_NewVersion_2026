using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Queries
{
    public static class GetGradeTransactions
    {
        public record Query(int GradeId) : IRequest<List<GradeTransactionDto>>;

        public class Handler : IRequestHandler<Query, List<GradeTransactionDto>>
        {
            private readonly IGradeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<GradeTransactionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var transactions = await _repo.GetTransactionsByGradeIdAsync(request.GradeId);

                return transactions.Select(t => new GradeTransactionDto(
                    Id: t.Id,
                    GradeId: t.GradeId,
                    TransactionTypeId: t.TransactionTypeId,
                    TransactionTypeName: lang == 2 ? t.TransactionType?.ArbName : t.TransactionType?.EngName,
                    CompanyId: t.CompanyId,
                    CompanyName: lang == 2 ? t.Company?.ArbName : t.Company?.EngName,
                    MinValue: t.MinValue,
                    MaxValue: t.MaxValue,
                    PaidAtVacation: t.PaidAtVacation,
                    OnceAtPeriod: t.OnceAtPeriod,
                    IntervalId: t.IntervalId,
                    IntervalName: lang == 2 ? t.Interval?.ArbName : t.Interval?.EngName,
                    NumberOfTickets: t.NumberOfTickets,
                    Remarks: t.Remarks,
                    RegDate: t.RegDate,
                    CancelDate: t.CancelDate,
                    IsActive: t.IsActive()
                )).ToList();
            }
        }
    }
}