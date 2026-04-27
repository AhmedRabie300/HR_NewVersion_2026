using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Grades.Queries
{
    public static class GetGradeStepById
    {
        public record Query(int Id) : IRequest<GradeStepDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;
       
            }
        }

        public class Handler : IRequestHandler<Query, GradeStepDto>
        {
            private readonly IGradeStepRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeStepRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<GradeStepDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
                

                return new GradeStepDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    GradeId: entity.GradeId,
                    GradeName: lang == 2 ? entity.Grade?.ArbName : entity.Grade?.EngName,
                    Step: entity.Step,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive(),
                    Transactions: entity.Transactions.Select(t => new GradeStepTransactionDto(
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
                );
            }
        }
    }
}