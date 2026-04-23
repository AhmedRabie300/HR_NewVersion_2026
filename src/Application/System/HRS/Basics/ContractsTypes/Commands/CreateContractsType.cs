using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.ContractsTypes.Dtos;
using Application.System.HRS.Basics.ContractsTypes.Validators;
using Domain.System.HRS.Basics.ContractsTypes;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.ContractsTypes.Commands
{
    public static class CreateContractsType
    {
        public record Command(CreateContractsTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IContractsTypeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateContractsTypeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new ContractsType(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    companyId: companyId,
                    isSpecial: request.Data.IsSpecial,
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