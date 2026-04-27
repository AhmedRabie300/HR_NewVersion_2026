using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.System.HRS.Basics.FiscalPeriod.Validators;
using Domain.System.HRS.Basics.FiscalPeriod;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Commands
{
    public static class CreateFiscalYearPeriod
    {
        public record Command(CreateFiscalYearPeriodDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IFiscalYearPeriodRepository repo, IFiscalYearRepository fiscalYearRepo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateFiscalYearPeriodValidator(msg, repo, fiscalYearRepo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IFiscalYearPeriodRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearPeriodRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new FiscalYearPeriod(
                    code: request.Data.Code,
                    fiscalYearId: request.Data.FiscalYearId,
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
                    prepareToDate: request.Data.PrepareToDate,
                    companyId: companyId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}