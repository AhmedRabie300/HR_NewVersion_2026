using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Groups.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Queries
{
    public static class GetGroupById
    {
        public record Query(int Id) : IRequest<GroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, GroupDto>
        {
            private readonly IGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<GroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var group = await _repo.GetByIdAsync(request.Id);
                if (group == null)
                    throw new NotFoundException(_msg.NotFound("Group", request.Id));

                return new GroupDto(
                    Id: group.Id,
                    Code: group.Code,
                    EngName: group.EngName,
                    ArbName: group.ArbName,
                    RegDate: group.RegDate,
                    CancelDate: group.CancelDate
                );
            }
        }
    }
}
