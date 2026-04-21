using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Validators;
using Domain.System.HRS.VacationAndEndOfService;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Commands
{
    public static class AddEndOfServiceRule
    {
        public record Command(int EndOfServiceId, CreateEndOfServiceRuleDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEndOfServiceRepository repo)
            {
                RuleFor(x => x.EndOfServiceId)
                    .GreaterThan(0).WithMessage(x => msg.Get("EndOfServiceIdRequired"))
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("EndOfService"), x.EndOfServiceId));

                RuleFor(x => x.Data)
                    .SetValidator(new CreateEndOfServiceRuleValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEndOfServiceRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IEndOfServiceRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var endOfService = await _repo.GetByIdAsync(request.EndOfServiceId);
                if (endOfService == null)
                    throw new NotFoundException(_msg.NotFound("EndOfService", request.EndOfServiceId));

                var rule = new EndOfServiceRule(
                    endOfServiceId: request.EndOfServiceId,
                    fromWorkingMonths: request.Data.FromWorkingMonths,
                    toWorkingMonths: request.Data.ToWorkingMonths,
                    amountPercent: request.Data.AmountPercent,
                    formula: request.Data.Formula,
                    extraDedFormula: request.Data.ExtraDedFormula,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddRuleAsync(rule);
                await _repo.SaveChangesAsync(cancellationToken);

                return rule.Id;
            }
        }
    }
}