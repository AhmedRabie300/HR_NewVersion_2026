using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Commands
{
    public static class GenerateFiscalYearPeriods
    {
        public record Command(int FiscalYearId, bool IsFormal, int? SourceFiscalYearId = null) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IFiscalYearRepository _fiscalYearRepo;

            public Validator(IFiscalYearRepository fiscalYearRepo)
            {
                _fiscalYearRepo = fiscalYearRepo;

                RuleFor(x => x.FiscalYearId)
                    .GreaterThan(0).WithMessage("FiscalYearId is required")
                    .MustAsync(async (id, ct) => await _fiscalYearRepo.ExistsAsync(id))
                    .WithMessage("Fiscal year not found");

                RuleFor(x => x)
                    .Must(x => x.IsFormal == true || x.SourceFiscalYearId.HasValue)
                    .WithMessage("If not formal, SourceFiscalYearId is required");
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IFiscalYearPeriodRepository _repo;

            public Handler(IFiscalYearPeriodRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var periods = await _repo.GeneratePeriodsAsync(request.FiscalYearId, request.IsFormal, request.SourceFiscalYearId, cancellationToken);

                foreach (var period in periods)
                {
                    await _repo.AddAsync(period);
                }

                await _repo.SaveChangesAsync(cancellationToken);

                return periods.Count;
            }
        }
    }
}