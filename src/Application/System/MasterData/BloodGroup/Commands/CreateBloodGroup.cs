using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class CreateBloodGroup
    {
        public record Command(CreateBloodGroupDto Data) : IRequest<int>;

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
                    .SetValidator(new CreateBloodGroupValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
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

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("BloodGroup", lang),
                        request.Data.Code));

                var bloodGroup = new Domain.System.MasterData.BloodGroup(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(bloodGroup);
                await _repo.SaveChangesAsync(cancellationToken);

                return bloodGroup.Id;
            }
        }
    }
}