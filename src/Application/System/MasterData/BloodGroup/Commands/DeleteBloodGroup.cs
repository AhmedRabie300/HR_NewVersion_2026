using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class DeleteBloodGroup
    {
        public record Command(int Id, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBloodGroupRepository _repo;

            public Handler(IBloodGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.ExistsAsync(request.Id))
                     throw new NotFoundException("Delete Blood Group",$"BloodGroup with Id {request.Id} not found.");

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return request.Id;
            }
        }
    }
}