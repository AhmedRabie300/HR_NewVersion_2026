using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class UpdateBloodGroup
    {
        public record Command(UpdateBloodGroupDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                // ✅ استخدام SetValidator مباشرة (من غير Custom)
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateBloodGroupValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBloodGroupRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(IBloodGroupRepository repo, IContextService ContextService, ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                var bloodGroup = await _repo.GetByIdAsync(request.Data.Id);
                if (bloodGroup == null)
                    throw new NotFoundException("Update Blood Group",string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("BloodGroup", lang),
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