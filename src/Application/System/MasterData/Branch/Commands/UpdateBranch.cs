using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Branch.Commands
{
    public static class UpdateBranch
    {
        public record Command(UpdateBranchDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateBranchValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBranchRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IBranchRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

          

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
              

                var branch = await _repo.GetByIdAsync(request.Data.Id);
                if (branch == null)
                    throw new NotFoundException(_msg.NotFound("Branch", request.Data.Id));
            
                    branch.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
              

                // Update location
                if (request.Data.CountryId.HasValue || request.Data.CityId.HasValue)
                {
                    branch.UpdateLocation(
                        request.Data.CountryId,
                        request.Data.CityId
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId != branch.Id)
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent is null)
                            throw new NotFoundException(_msg.NotFound("ParentBranch", request.Data.Id));
                        branch.UpdateParent(request.Data.ParentId);
                    }
                }

                // Update settings
                if (request.Data.DefaultAbsent.HasValue ||
                    request.Data.PrepareDay.HasValue ||
                    request.Data.AffectPeriod.HasValue)
                {
                    branch.UpdateSettings(
                        request.Data.DefaultAbsent,
                        request.Data.PrepareDay,
                        request.Data.AffectPeriod
                    );
                }

                await _repo.UpdateAsync(branch);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}