using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Commands
{
    public static class CreateBranch
    {
        public record Command(CreateBranchDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IBranchRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateBranchValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBranchRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler( IBranchRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var code = await _repo.CodeExistsAsync(request.Data.Code);
                if(code)
                {
                    throw new ConflictException(_msg.CodeExists("Branch", request.Data.Code));
                }

                var entity = new Domain.System.MasterData.Branch(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    countryId: request.Data.CountryId,
                    cityId: request.Data.CityId,
                    defaultAbsent: request.Data.DefaultAbsent,
                    prepareDay: request.Data.PrepareDay,
                    affectPeriod: request.Data.AffectPeriod,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}