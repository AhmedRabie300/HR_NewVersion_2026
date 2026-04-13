using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Queries
{
    public static class GetModulePermissionById
    {
        public record Query(int Id) : IRequest<ModulePermissionDto>;

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

        public class Handler : IRequestHandler<Query, ModulePermissionDto>
        {
            private readonly IModulePermissionRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IModulePermissionRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<ModulePermissionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var permission = await _repo.GetByIdAsync(request.Id);
                if (permission == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("ModulePermission", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("ModulePermission", lang), request.Id));

                return new ModulePermissionDto(
                    Id: permission.Id,
                    ModuleId: permission.ModuleId,
                    ModuleCode: permission.Module?.Code,
                    ModuleName: lang == 2 ? (permission.Module?.ArbName ?? permission.Module?.EngName) : (permission.Module?.EngName ?? permission.Module?.ArbName),
                    GroupId: permission.GroupId,
                    GroupCode: permission.Group?.Code,
                    GroupName: lang == 2 ? (permission.Group?.ArbName ?? permission.Group?.EngName) : (permission.Group?.EngName ?? permission.Group?.ArbName),
                    UserId: permission.UserId,
                    UserCode: permission.User?.Code,
                    UserName: lang == 2 ? (permission.User?.ArbName ?? permission.User?.EngName) : (permission.User?.EngName ?? permission.User?.ArbName),
                    CanView: permission.CanView,
                    RegDate: permission.RegDate,
                    CancelDate: permission.CancelDate,
                    IsActive: permission.IsActive()
                );
            }
        }
    }
}