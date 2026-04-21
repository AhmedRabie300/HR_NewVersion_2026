using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Commands
{
    public static class UpdateEndOfServiceRule
    {
        public record Command(UpdateEndOfServiceRuleDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEndOfServiceRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateEndOfServiceRuleValidator(msg));

                RuleFor(x => x.Data.EndOfServiceId)
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("EndOfService"), x.Data.EndOfServiceId));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEndOfServiceRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IEndOfServiceRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var rule = await _repo.GetRuleByIdAsync(request.Data.Id);
                if (rule == null)
                    throw new NotFoundException(_msg.NotFound("EndOfServiceRule", request.Data.Id));

                rule.Update(
                    fromWorkingMonths: request.Data.FromWorkingMonths,
                    toWorkingMonths: request.Data.ToWorkingMonths,
                    amountPercent: request.Data.AmountPercent,
                    formula: request.Data.Formula,
                    extraDedFormula: request.Data.ExtraDedFormula,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateRuleAsync(rule);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}