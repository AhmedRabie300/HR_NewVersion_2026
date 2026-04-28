using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalYears.Dtos;
using Application.System.HRS.Basics.FiscalYears.Validators;
using Domain.System.HRS.Basics;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalYears.Commands
{
    public static class CreateFiscalYear
    {
        public record Command(CreateFiscalYearDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IFiscalYearRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateFiscalYearValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IFiscalYearRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new Domain.System.HRS.Basics.FiscalPeriod.FiscalYear(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}