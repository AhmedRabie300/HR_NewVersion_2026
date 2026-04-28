using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.System.HRS.Basics.FiscalPeriod.Validators;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Commands
{
    public static class UpdateFiscalYearPeriodModule
    {
        public record Command(UpdateFiscalYearPeriodModuleDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IFiscalYearPeriodRepository repo, IModuleRepository moduleRepo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateFiscalYearPeriodModuleValidator(msg, repo, moduleRepo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IFiscalYearPeriodRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IFiscalYearPeriodRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetModuleByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("FiscalYearPeriodModule", request.Data.Id));

                entity.Update(
                    openDate: request.Data.OpenDate,
                    closeDate: request.Data.CloseDate,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateModuleAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}