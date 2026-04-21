using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Queries
{
    public static class GetEndOfServiceRules
    {
        public record Query(int EndOfServiceId) : IRequest<List<EndOfServiceRuleDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

                RuleFor(x => x.EndOfServiceId)
                    .GreaterThan(0).WithMessage(x => _currentUser.Language == 2
                        ? "معرف خدمة نهاية الخدمة يجب أن يكون أكبر من 0"
                        : "EndOfServiceId must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, List<EndOfServiceRuleDto>>
        {
            private readonly IEndOfServiceRepository _repo;

            public Handler(IEndOfServiceRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<EndOfServiceRuleDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var rules = await _repo.GetRulesByEndOfServiceIdAsync(request.EndOfServiceId);

                return rules.Select(r => new EndOfServiceRuleDto(
                    Id: r.Id,
                    EndOfServiceId: r.EndOfServiceId,
                    FromWorkingMonths: r.FromWorkingMonths,
                    ToWorkingMonths: r.ToWorkingMonths,
                    AmountPercent: r.AmountPercent,
                    Formula: r.Formula,
                    ExtraDedFormula: r.ExtraDedFormula,
                    Remarks: r.Remarks,
                    RegDate: r.RegDate,
                    CancelDate: r.CancelDate,
                    IsActive: r.IsActive()
                )).ToList();
            }
        }
    }
}