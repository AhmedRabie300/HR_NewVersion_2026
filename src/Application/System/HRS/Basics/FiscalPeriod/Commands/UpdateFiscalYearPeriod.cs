using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.System.HRS.Basics.FiscalPeriod.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Commands
{
    public static class UpdateFiscalYearPeriod
    {
        public record Command(UpdateFiscalYearPeriodDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IFiscalYearPeriodRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateFiscalYearPeriodValidator(msg, repo));
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
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("FiscalYearPeriod", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    fromDate: request.Data.FromDate,
                    toDate: request.Data.ToDate,
                    remarks: request.Data.Remarks,
                    hFromDate: request.Data.HFromDate,
                    hToDate: request.Data.HToDate,
                    periodType: request.Data.PeriodType,
                    periodRank: request.Data.PeriodRank,
                    prepareFromDate: request.Data.PrepareFromDate,
                    prepareToDate: request.Data.PrepareToDate
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}