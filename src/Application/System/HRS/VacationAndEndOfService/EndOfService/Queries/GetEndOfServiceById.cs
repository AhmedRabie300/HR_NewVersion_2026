using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Queries
{
    public static class GetEndOfServiceById
    {
        public record Query(int Id) : IRequest<EndOfServiceDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

        
            }
        }

        public class Handler : IRequestHandler<Query, EndOfServiceDto>
        {
            private readonly IEndOfServiceRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IEndOfServiceRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<EndOfServiceDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
 

                return new EndOfServiceDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    ExtraTransactionId: entity.ExtraTransactionId,
                    ExcludedFromSSRequests: entity.ExcludedFromSSRequests,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive(),
                    Rules: entity.Rules.Select(r => new EndOfServiceRuleDto(
                        r.Id,
                        r.EndOfServiceId,
                        r.FromWorkingMonths,
                        r.ToWorkingMonths,
                        r.AmountPercent,
                        r.Formula,
                        r.ExtraDedFormula,
                        r.Remarks,
                        r.RegDate,
                        r.CancelDate,
                        r.IsActive()
                    )).ToList()
                );
            }
        }
    }
}