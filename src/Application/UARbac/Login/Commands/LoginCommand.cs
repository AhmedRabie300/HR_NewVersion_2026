// Application/UARbac/Login/Commands/LoginCommand.cs
using Application.UARbac.Abstractions;
using Application.UARbac.Login.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UARbac.Login.Commands
{
    public static class Login
    {
        public sealed record Command(LoginRequestDto Request, int Lang) : IRequest<LoginResponseDto>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Request.Username)
                    .NotEmpty()
                    .WithMessage("Username is required");

                RuleFor(x => x.Request.Password)
                    .NotEmpty()
                    .WithMessage("Password is required");

                RuleFor(x => x.Lang)
                    .Must(x => x == 1 || x == 2)
                    .WithMessage("Language must be 1 (English) or 2 (Arabic)");
            }
        }

        public sealed class Handler : IRequestHandler<Command, LoginResponseDto>
        {
            private readonly ILoginRepository _repo;
            //private readonly IConfiguration _configuration;
            //private readonly IJwtService _jwtService;
            //private readonly IEncryptionService _encryptionService;
            //private readonly IUserGroupRepository _userGroupRepo;
            //private readonly IMenuRepository _menuRepo;
            //private readonly IFormPermissionRepository _permissionRepo;

            public Handler(ILoginRepository repo)
            {
                _repo = repo;
            }

            public async Task<LoginResponseDto> Handle(Command request, CancellationToken ct)
            {
                return await _repo.LoginAsync(request.Request, request.Lang, ct);
            }
        }
    }
}