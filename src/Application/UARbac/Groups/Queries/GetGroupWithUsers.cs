using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Queries
{
    public static class GetGroupWithUsers
    {
        public record Query(int Id) : IRequest<GroupWithUsersDto>;

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

        public class Handler : IRequestHandler<Query, GroupWithUsersDto>
        {
            private readonly IGroupRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IGroupRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<GroupWithUsersDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var group = await _repo.GetByIdAsync(request.Id);
                if (group == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Group", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Group", lang), request.Id));

                var userGroups = await _repo.GetGroupUsersAsync(request.Id);

                var users = userGroups.Select(ug => new GroupUserDto(
                    UserId: ug.UserId,
                    UserCode: ug.User?.Code ?? "",
                    UserName: lang == 2 ? (ug.User?.ArbName ?? ug.User?.EngName ?? "") : (ug.User?.EngName ?? ug.User?.ArbName ?? "")
                    
                )).ToList();

                return new GroupWithUsersDto(
                    Id: group.Id,
                    Code: group.Code,
                    EngName: group.EngName,
                    ArbName: group.ArbName,
                    RegDate: group.RegDate,
                    CancelDate: group.CancelDate,
                    UsersCount: users.Count,
                    Users: users
                );
            }
        }
    }
}