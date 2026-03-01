using MediatR;
using VenusHR.Application.Common.DTOs.Users;
using VenusHR.Application.Common.DTOs.Shared;   

namespace VenusHR.Application.Common.Interfaces.Users
{
    public static class UserCommands
    {
         public record CreateUserCommand(UserDto Dto) : IRequest<ApiResponse<UserDto>>;
        public record UpdateUserCommand(int Id, UpdateUserDto Dto) : IRequest<ApiResponse<UserDto>>;
        public record DeleteUserCommand(int Id) : IRequest<ApiResponse<bool>>;

         public record AddUserToGroupCommand(int UserId, int GroupId, bool IsPrimary) : IRequest<ApiResponse<bool>>;
        public record RemoveUserFromGroupCommand(int UserId, int GroupId) : IRequest<ApiResponse<bool>>;

         public record UpdateUserStatusCommand(int UserId, bool IsActive) : IRequest<ApiResponse<bool>>;
        public record UpdateDeviceTokenCommand(int UserId, string DeviceToken) : IRequest<ApiResponse<bool>>;

         public class Handler :
            IRequestHandler<CreateUserCommand, ApiResponse<UserDto>>,
            IRequestHandler<UpdateUserCommand, ApiResponse<UserDto>>,
            IRequestHandler<DeleteUserCommand, ApiResponse<bool>>,
            IRequestHandler<AddUserToGroupCommand, ApiResponse<bool>>,
            IRequestHandler<RemoveUserFromGroupCommand, ApiResponse<bool>>,
            IRequestHandler<UpdateUserStatusCommand, ApiResponse<bool>>,
            IRequestHandler<UpdateDeviceTokenCommand, ApiResponse<bool>>
        {
            private readonly IUserService _userService;

            public Handler(IUserService userService)
            {
                _userService = userService;
            }

             public async Task<ApiResponse<UserDto>> Handle(CreateUserCommand cmd, CancellationToken ct)
                => await _userService.CreateUserAsync(cmd.Dto);

            public async Task<ApiResponse<UserDto>> Handle(UpdateUserCommand cmd, CancellationToken ct)
                => await _userService.UpdateUserAsync(cmd.Id, cmd.Dto);

            public async Task<ApiResponse<bool>> Handle(DeleteUserCommand cmd, CancellationToken ct)
                => await _userService.DeleteUserAsync(cmd.Id);

             public async Task<ApiResponse<bool>> Handle(AddUserToGroupCommand cmd, CancellationToken ct)
                => await _userService.AddUserToGroupAsync(cmd.UserId, cmd.GroupId, cmd.IsPrimary);

            public async Task<ApiResponse<bool>> Handle(RemoveUserFromGroupCommand cmd, CancellationToken ct)
                => await _userService.RemoveUserFromGroupAsync(cmd.UserId, cmd.GroupId);

             public async Task<ApiResponse<bool>> Handle(UpdateUserStatusCommand cmd, CancellationToken ct)
                => await _userService.UpdateUserStatusAsync(cmd.UserId, cmd.IsActive);

            public async Task<ApiResponse<bool>> Handle(UpdateDeviceTokenCommand cmd, CancellationToken ct)
                => await _userService.UpdateDeviceTokenAsync(cmd.UserId, cmd.DeviceToken);
        }
    }
}