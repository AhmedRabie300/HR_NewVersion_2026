using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Users.Dtos;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using FluentValidation;
using MediatR;
using UserEntity = Domain.UARbac.Users;

namespace Application.UARbac.Users.Commands
{
    public static class Create
    {
        public record Command(CreateUserDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                var lang = _contextService.GetCurrentLanguage();

                RuleFor(x => x.Data.Code)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithMessage(string.Format(_localizer.GetMessage("CodeRequired", lang)));

                RuleFor(x => x.Data.Password)
                    .NotEmpty()
                    .WithMessage(_localizer.GetMessage("PasswordRequired", lang));

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(100)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(100)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));
            }
        }

        public class Handler : IRequestHandler<Command, int>
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

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var existingUser = await _repo.GetByCodeAsync(request.Data.Code);
                if (existingUser != null)
                    throw new ConflictException(
                        _localizer.GetMessage("User", lang),
                        "Code",
                        request.Data.Code,
                        string.Format(_localizer.GetMessage("CodeExists", lang), _localizer.GetMessage("User", lang), request.Data.Code));

                var user = new UserEntity(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    password: request.Data.Password,
                    isAdmin: request.Data.IsAdmin,
                    isArabic: request.Data.IsArabic,
                    canChangePassword: request.Data.CanChangePassword,
                    resetSearchCriteria: request.Data.ResetSearchCriteria,
                    resetReportCriteria: request.Data.ResetReportCriteria,
                    sessionIdleTime: request.Data.SessionIdleTime,
                    enforceAlphaNumericPwd: request.Data.EnforceAlphaNumericPwd,
                    passwordExpiry: request.Data.PasswordExpiry,
                    remarks: request.Data.Remarks,
                    relEmployee: request.Data.RelEmployee,
                    levelId: request.Data.LevelId,
                    deviceToken: request.Data.DeviceToken,
                    isActive: request.Data.IsActive
                );

                var createdUser = await _repo.AddAsync(user);
                await _repo.SaveChangesAsync(cancellationToken);

                if (request.Data.GroupIds != null && request.Data.GroupIds.Any())
                {
                    foreach (var groupId in request.Data.GroupIds)
                    {
                        await _repo.AddUserToGroupAsync(createdUser.Id, groupId, false);
                    }
                    await _repo.SaveChangesAsync(cancellationToken);
                }

                return createdUser.Id;
            }
        }
    }
}