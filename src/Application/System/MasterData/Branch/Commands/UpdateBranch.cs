using Application.Common;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Commands
{
    public static class UpdateBranch
    {
        public record Command(UpdateBranchDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateBranchValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBranchRepository _repo;

            public Handler(IBranchRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var branch = await _repo.GetByIdAsync(request.Data.Id);
                if (branch == null)
                    throw new NotFoundException("Update Branch",$"Branch with ID {request.Data.Id} not found");

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    branch.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

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
                    // Check if parent exists
                    if (request.Data.ParentId != branch.Id) // Prevent self-reference
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent == null)
                            throw new NotFoundException("Update Branch",$"Parent branch with ID {request.Data.ParentId} not found");

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