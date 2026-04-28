using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.System.HRS.Basics.FiscalPeriod.Validators;
using Application.UARbac.Abstractions;
using Domain.System.HRS.Basics.FiscalPeriod;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Commands
{
    public static class AddFiscalYearPeriodModule
    {
        public record Command(CreateFiscalYearPeriodModuleDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IFiscalYearPeriodRepository repo, IModuleRepository moduleRepo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateFiscalYearPeriodModuleValidator(msg, repo, moduleRepo));
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

                var entity = new FiscalYearPeriodModule(
                    fiscalYearPeriodId: request.Data.FiscalYearPeriodId,
                    moduleId: request.Data.ModuleId,
                    companyId: companyId,
                    openDate: request.Data.OpenDate,
                    closeDate: request.Data.CloseDate,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddModuleAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}