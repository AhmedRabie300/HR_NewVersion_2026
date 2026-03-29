using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.System.MasterData.BloodGroup.Queries
{
    public static class GetBloodGroupById
    {
        public record Query(int Id, int Lang = 1) : IRequest<BloodGroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, BloodGroupDto>
        {
            private readonly IBloodGroupRepository _repo;

            public Handler(IBloodGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<BloodGroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var bloodGroup = await _repo.GetByIdAsync(request.Id);
                if (bloodGroup == null)
                    throw new NotFoundException("Get Blood Group",$"BloodGroup with ID {request.Id} not found");

                return new BloodGroupDto(
                    Id: bloodGroup.Id,
                    Code: bloodGroup.Code,
                    EngName: bloodGroup.EngName,
                    ArbName: bloodGroup.ArbName,
                    ArbName4S: bloodGroup.ArbName4S,
                    Remarks: bloodGroup.Remarks,
                    RegDate: bloodGroup.RegDate,
                    CancelDate: bloodGroup.CancelDate,
                    IsActive: bloodGroup.IsActive()
                );
            }
        }
    }
}