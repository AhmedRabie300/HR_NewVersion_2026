using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;

using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Queries
{
    public static class GetGradeById
    {
        public record Query(int Id) : IRequest<GradeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;
               
            }
        }

        public class Handler : IRequestHandler<Query, GradeDto>
        {
            private readonly IGradeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<GradeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
              

                return new GradeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    GradeLevel: entity.GradeLevel,
                    FromSalary: entity.FromSalary,
                    ToSalary: entity.ToSalary,
                    RegularHours: entity.RegularHours,
                    OverTimeTypeId: entity.OverTimeTypeId,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive(),
                    Transactions: entity.Transactions.Select(t => new GradeTransactionDto(
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
                );
            }
        }
    }
}