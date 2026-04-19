using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Users.Dtos;
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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data.Code)
                    .NotEmpty().WithMessage(msg.Get("CodeRequired"))
                    .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

                RuleFor(x => x.Data.Password)
                    .NotEmpty().WithMessage(msg.Get("PasswordRequired"));

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IUserRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IUserRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingUser = await _repo.GetByCodeAsync(request.Data.Code);
                if (existingUser != null)
                    throw new ConflictException(_msg.CodeExists("User", request.Data.Code));

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
