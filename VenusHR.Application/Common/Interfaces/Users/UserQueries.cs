using MediatR;
using VenusHR.Application.Common.DTOs.Users;
using VenusHR.Application.Common.DTOs.Groups;
using VenusHR.Application.Common.DTOs.Login;   
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.Interfaces.Users;

namespace VenusHR.Application.Common.Interfaces.Users
{
    public static class UserQueries
    {
         public record GetAllUsersQuery() : IRequest<ApiResponse<List<UserDto>>>;
        public record GetUserByIdQuery(int Id) : IRequest<ApiResponse<UserDto>>;

         public record GetUserGroupsQuery(int UserId) : IRequest<ApiResponse<List<UserGroupDto>>>;

 
        public record GetUserFeaturesQuery(int UserId) : IRequest<ApiResponse<List<UserFeatureDto>>>;
        public record GetUserFeaturesByGroupQuery(int UserId, int GroupId) : IRequest<ApiResponse<List<UserFeatureDto>>>;

         public record CheckIsAdminQuery(int UserId) : IRequest<ApiResponse<bool>>;
        public record CheckIsActiveQuery(int UserId) : IRequest<ApiResponse<bool>>;

         public class Handler :
            IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserDto>>>,
            IRequestHandler<GetUserByIdQuery, ApiResponse<UserDto>>,
            IRequestHandler<GetUserGroupsQuery, ApiResponse<List<UserGroupDto>>>,
            IRequestHandler<GetUserFeaturesQuery, ApiResponse<List<UserFeatureDto>>>,   
            IRequestHandler<GetUserFeaturesByGroupQuery, ApiResponse<List<UserFeatureDto>>>,   
            IRequestHandler<CheckIsAdminQuery, ApiResponse<bool>>,
            IRequestHandler<CheckIsActiveQuery, ApiResponse<bool>>
        {
            private readonly IUserService _userService;

            public Handler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<ApiResponse<List<UserDto>>> Handle(GetAllUsersQuery query, CancellationToken ct)
                => await _userService.GetAllUsersAsync();

            public async Task<ApiResponse<UserDto>> Handle(GetUserByIdQuery query, CancellationToken ct)
                => await _userService.GetUserByIdAsync(query.Id);

            public async Task<ApiResponse<List<UserGroupDto>>> Handle(GetUserGroupsQuery query, CancellationToken ct)
                => await _userService.GetUserGroupsAsync(query.UserId);

             public async Task<ApiResponse<List<UserFeatureDto>>> Handle(GetUserFeaturesQuery query, CancellationToken ct)
                => await _userService.GetUserFeaturesAsync(query.UserId);

             public async Task<ApiResponse<List<UserFeatureDto>>> Handle(GetUserFeaturesByGroupQuery query, CancellationToken ct)
                => await _userService.GetUserFeaturesByGroupAsync(query.UserId, query.GroupId);

            public async Task<ApiResponse<bool>> Handle(CheckIsAdminQuery query, CancellationToken ct)
                => await _userService.IsAdminAsync(query.UserId);

            public async Task<ApiResponse<bool>> Handle(CheckIsActiveQuery query, CancellationToken ct)
                => await _userService.IsActiveAsync(query.UserId);
        }
    }
}