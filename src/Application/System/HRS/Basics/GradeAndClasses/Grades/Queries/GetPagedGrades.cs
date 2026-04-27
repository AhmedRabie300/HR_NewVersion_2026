using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.Grades.Queries
{
    public static class GetPagedGrades
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null
        ) : IRequest<PagedResult<GradeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<GradeDto>>
        {
            private readonly IGradeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<GradeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new GradeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    GradeLevel: x.GradeLevel,
                    FromSalary: x.FromSalary,
                    ToSalary: x.ToSalary,
                    RegularHours: x.RegularHours,
                    OverTimeTypeId: x.OverTimeTypeId,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive(),
                    Transactions: x.Transactions.Select(t => new GradeTransactionDto(
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
                    )).ToList()
                )).ToList();

                return new PagedResult<GradeDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}