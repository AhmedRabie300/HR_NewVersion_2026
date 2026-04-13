using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Users.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Users.Queries
{
    public static class GetUserById
    {
        public record Query(int Id) : IRequest<GetUserDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, GetUserDto>
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

            public async Task<GetUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var user = await _repo.GetByIdAsync(request.Id);
                if (user == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("User", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("User", lang), request.Id));

                return new GetUserDto(
                    Id: user.Id,
                    Code: user.Code,
                    EngName: user.EngName,
                    ArbName: user.ArbName,
                    ArbName4S: user.ArbName4S,
                    IsAdmin: user.IsAdmin,
                    IsArabic: user.IsArabic,
                    CanChangePassword: user.CanChangePassword,
                    ResetSearchCriteria: user.ResetSearchCriteria,
                    ResetReportCriteria: user.ResetReportCriteria,
                    SessionIdleTime: user.SessionIdleTime,
                    EnforceAlphaNumericPwd: user.EnforceAlphaNumericPwd,
                    PasswordExpiry: user.PasswordExpiry,
                    PasswordChangedOn: user.PasswordChangedOn,
                    Remarks: user.Remarks,
                    regComputerId: user.regComputerId,
                    RegDate: user.RegDate,
                    CancelDate: user.CancelDate,
                    RelEmployee: user.RelEmployee,
                    LevelId: user.LevelId,
                    DeviceToken: user.DeviceToken,
                    IsActive: user.IsActive
                );
            }
        }
    }
}