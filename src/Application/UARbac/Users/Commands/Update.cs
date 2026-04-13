using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Modules.Validators;
using Application.UARbac.Users.Dtos;
using Application.UARbac.Users.Validators;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Users.Commands
{
    public static class Update
    {
        public record Command(UpdateUserDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                var lang = _contextService.GetCurrentLanguage();

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateUserValidator(_contextService, _localizer));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUserRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IUserRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var user = await _repo.GetByIdAsync(request.Data.Id);
                if (user == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("User", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("User", lang), request.Data.Id));

                if (request.Data.EngName != null)
                    user.UpdatePersonalInfo(request.Data.EngName, null, null, null);

                if (request.Data.ArbName != null)
                    user.UpdatePersonalInfo(null, request.Data.ArbName, null, null);

                if (request.Data.IsAdmin != null)
                    user.UpdatePermissions(request.Data.IsAdmin, null, null);

                if (request.Data.DeviceToken != null)
                    user.UpdateDeviceToken(request.Data.DeviceToken);

                await _repo.UpdateAsync(user);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}