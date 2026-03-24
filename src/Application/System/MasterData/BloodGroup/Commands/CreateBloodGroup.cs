using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Validators;
using Application.Common.Abstractions;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class CreateBloodGroup
    {
        public record Command(CreateBloodGroupDto Data, int Lang = 1) : IRequest<int>;

      

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBloodGroupRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IBloodGroupRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new Exception(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("BloodGroup", request.Lang),
                        request.Data.Code));

                var bloodGroup = new Domain.System.MasterData.BloodGroup(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.RegComputerId
                );

                await _repo.AddAsync(bloodGroup);
                await _repo.SaveChangesAsync(cancellationToken);

                return bloodGroup.Id;
            }
        }
    }
}