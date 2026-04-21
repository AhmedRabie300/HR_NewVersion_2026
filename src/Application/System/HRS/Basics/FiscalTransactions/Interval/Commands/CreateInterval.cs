using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Validators;
using Domain.System.HRS.Basics.FiscalTransactions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Commands
{
    public static class CreateInterval
    {
        public record Command(CreateIntervalDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IIntervalRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateIntervalValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IIntervalRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IIntervalRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new Domain.System.HRS.Basics.FiscalTransactions.Interval(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    number: request.Data.Number,
                    companyId: _currentUser.CompanyId,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}