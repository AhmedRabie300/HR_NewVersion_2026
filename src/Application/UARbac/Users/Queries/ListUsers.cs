using Application.UARbac.Users.Dtos;
using Application.UARbac.Abstractions;
using MediatR;

namespace Application.UARbac.Users.Queries
{
    public static class ListUsers
    {
        public record Query : IRequest<List<GetUserDto>>;

        public class Handler : IRequestHandler<Query, List<GetUserDto>>
        {
            private readonly IUserRepository _repo;

            public Handler(IUserRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<GetUserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _repo.GetAllAsync();

                return users.Select(user => new GetUserDto(
                    user.Id,
                    user.Code,
                    user.EngName,
                    user.ArbName,
                    user.ArbName4S,
                    user.IsAdmin,
                    user.IsArabic,
                    user.CanChangePassword,
                    user.ResetSearchCriteria,
                    user.ResetReportCriteria,
                    user.SessionIdleTime,
                    user.EnforceAlphaNumericPwd,
                    user.PasswordExpiry,
                    user.PasswordChangedOn,
                    user.Remarks,
                    user.regComputerId,
                    user.RegDate,
                    user.CancelDate,
                    user.RelEmployee,
                    user.LevelId,
                    user.DeviceToken,
                    user.IsActive
                )).ToList();
            }
        }
    }
}