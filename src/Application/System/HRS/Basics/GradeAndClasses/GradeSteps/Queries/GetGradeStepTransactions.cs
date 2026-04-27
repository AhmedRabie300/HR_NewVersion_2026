using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Queries
{
    public static class GetGradeStepTransactions
    {
        public record Query(int GradeStepId) : IRequest<List<GradeStepTransactionDto>>;

        public class Handler : IRequestHandler<Query, List<GradeStepTransactionDto>>
        {
            private readonly IGradeStepRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeStepRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<GradeStepTransactionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var transactions = await _repo.GetTransactionsByGradeStepIdAsync(request.GradeStepId);

                return transactions.Select(t => new GradeStepTransactionDto(
                    Id: t.Id,
                    GradeStepId: t.GradeStepId,
                    GradeTransactionId: t.GradeTransactionId,
                    CompanyId: t.CompanyId,
                    CompanyName: lang == 2 ? t.Company?.ArbName : t.Company?.EngName,
                    Amount: t.Amount,
                    Remarks: t.Remarks,
                    Active: t.Active,
                    ActiveDate: t.ActiveDate,
                    ActiveDateD: t.ActiveDateD,
                    RegDate: t.RegDate,
                    CancelDate: t.CancelDate,
                    IsActive: t.IsActive()
                )).ToList();
            }
        }
    }
}