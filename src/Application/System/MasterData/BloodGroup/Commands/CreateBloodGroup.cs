using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class CreateBloodGroup
    {
        public record Command(CreateBloodGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IBloodGroupRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateBloodGroupValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBloodGroupRepository _repo;
            private readonly IValidationMessages _msg;
            public Handler(
                IBloodGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("BloodGroup", request.Data.Code));
                }

                    var bloodGroup = new Domain.System.MasterData.BloodGroup(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(bloodGroup);
                await _repo.SaveChangesAsync(cancellationToken);

                return bloodGroup.Id;
            }
        }
    }
}