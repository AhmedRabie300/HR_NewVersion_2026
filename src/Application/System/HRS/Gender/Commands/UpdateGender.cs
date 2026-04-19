using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Gender.Dtos;
using Application.System.HRS.Gender.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Gender.Commands
{
    public static class UpdateGender
    {
        public record Command(UpdateGenderDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGenderValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGenderRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly IValidationMessages _msg;

            public Handler(
                IGenderRepository repo,
                IContextService contextService,
                ILocalizationService localizer,
                IValidationMessages msg)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity is null)
                    throw new NotFoundException(_msg.NotFound("Gender", request.Data.Id));


                 if (request.Data.Code != null && request.Data.Code != entity.Code)
                {
                    if (await _repo.CodeExistsAsync(request.Data.Code, request.Data.Id))
                        throw new ConflictException(string.Format(
                            _localizer.GetMessage("CodeExists", lang),
                            _localizer.GetMessage("Gender", lang),
                            request.Data.Code));
                }

                entity.Update(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}