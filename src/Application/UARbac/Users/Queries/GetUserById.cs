using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Users.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Users.Queries
{
    public static class GetUserById
    {
        public record Query(int Id) : IRequest<GetUserDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, GetUserDto>
        {
            private readonly IUserRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IUserRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<GetUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _repo.GetByIdAsync(request.Id);
                if (user == null)
                    throw new NotFoundException(_msg.NotFound("User", request.Id));

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
