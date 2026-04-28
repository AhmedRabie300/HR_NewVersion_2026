using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.Grades.Queries
{
    public static class GetPagedGradeSteps
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null,
            int? GradeId = null
        ) : IRequest<PagedResult<GradeStepDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<GradeStepDto>>
        {
            private readonly IGradeStepRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeStepRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<GradeStepDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.GradeId
                );

                var items = pagedResult.Items.Select(x => new GradeStepDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    GradeId: x.GradeId,
                    GradeName: lang == 2 ? x.Grade?.ArbName : x.Grade?.EngName,
                    Step: x.Step,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive(),
                    Transactions: x.Transactions.Select(t => new GradeStepTransactionDto(
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
                    )).ToList()
                )).ToList();

                return new PagedResult<GradeStepDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}