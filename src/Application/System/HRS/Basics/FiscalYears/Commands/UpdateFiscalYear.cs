using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalYears.Dtos;
using Application.System.HRS.Basics.FiscalYears.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalYears.Commands
{
    public static class UpdateFiscalYear
    {
        public record Command(UpdateFiscalYearDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IFiscalYearRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateFiscalYearValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IFiscalYearRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IFiscalYearRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("FiscalYear", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}