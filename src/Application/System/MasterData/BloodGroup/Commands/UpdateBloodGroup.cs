using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class UpdateBloodGroup
    {
        public record Command(UpdateBloodGroupDto Data, int Lang = 1) : IRequest<Unit>;

       

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBloodGroupRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IBloodGroupRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var bloodGroup = await _repo.GetByIdAsync(request.Data.Id);
                if (bloodGroup == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("BloodGroup", request.Lang),
                        request.Data.Id));

                bloodGroup.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(bloodGroup);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}