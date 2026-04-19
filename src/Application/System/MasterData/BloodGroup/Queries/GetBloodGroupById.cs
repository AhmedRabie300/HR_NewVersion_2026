using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;
using Application.Abstractions;

namespace Application.System.MasterData.BloodGroup.Queries
{
    public static class GetBloodGroupById
    {
        public record Query(int Id) : IRequest<BloodGroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, BloodGroupDto>
        {
            private readonly IBloodGroupRepository _repo;

                        private readonly IValidationMessages _msg;
public Handler(IBloodGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<BloodGroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var bloodGroup = await _repo.GetByIdAsync(request.Id);
                if (bloodGroup == null)
                    throw new NotFoundException(_msg.NotFound("BloodGroup", request.Id));

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