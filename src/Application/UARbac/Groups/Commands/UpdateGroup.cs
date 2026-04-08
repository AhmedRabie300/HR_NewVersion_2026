// Application/UARbac/Groups/Commands/UpdateGroup.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Department.Validators;
using Application.UARbac.Abstractions;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Groups.Validators;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Commands
{
    public static class UpdateGroup
    {
        public record Command(UpdateGroupDto Data) : IRequest<Unit>;

        //public sealed class Validator : AbstractValidator<Command>
        //{
        //    public Validator()
        //    {
        //        RuleFor(x => x.Data.Id)
        //            .GreaterThan(0)
        //            .WithMessage("Group ID is required");

        //        RuleFor(x => x.Data.EngName)
        //            .MaximumLength(200)
        //            .WithMessage("English name must not exceed 200 characters");

        //        RuleFor(x => x.Data.ArbName)
        //            .MaximumLength(200)
        //            .WithMessage("Arabic name must not exceed 200 characters");

        //        // At least one field must be provided
        //        RuleFor(x => x.Data)
        //            .Must(x => x.EngName != null || x.ArbName != null)
        //            .WithMessage("At least one field must be provided to update");
        //    }
        //}
        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGroupValidator(localizer, languageService));
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGroupRepository _repo;

            public Handler(IGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var group = await _repo.GetByIdAsync(request.Data.Id);
                if (group == null)
                    throw new NotFoundException("Update Group",$"Group with ID {request.Data.Id} not found");

                group.Update(request.Data.EngName, request.Data.ArbName);

                await _repo.UpdateAsync(group);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}