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
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("User ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, GetUserDto>
        {
            private readonly IUserRepository _repo;

            public Handler(IUserRepository repo)
            {
                _repo = repo;
            }

            public async Task<GetUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _repo.GetByIdAsync(request.Id);
                if (user == null)
                    throw new Exception($"User with ID {request.Id} not found");

                return new GetUserDto(
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
                    user.RegComputerId,
                    user.RegDate,
                    user.CancelDate,
                    user.RelEmployee,
                    user.LevelId,
                    user.DeviceToken,
                    user.IsActive
                );
            }
        }
    }
}