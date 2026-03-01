using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.Interfaces.Login;


namespace VenusHR.Application.Common.Interfaces.Login.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserLoginResponseDto>
    {
        private readonly ILoginServices _loginService;   

        public LoginCommandHandler(ILoginServices loginService)
        {
            _loginService = loginService;
        }

        public async Task<UserLoginResponseDto> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
             return await _loginService.LoginAsync(command.Request, command.Lang);
        }
    }
}
