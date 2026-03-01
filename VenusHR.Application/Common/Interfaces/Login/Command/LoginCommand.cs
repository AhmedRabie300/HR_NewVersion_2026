using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VenusHR.Application.Common.DTOs.Login;

namespace VenusHR.Application.Common.Interfaces.Login.Command
{
    public class LoginCommand : IRequest<UserLoginResponseDto>
    {
        public LoginRequestDto Request { get; set; }
        public int Lang { get; set; }

        public LoginCommand(LoginRequestDto request, int lang)
        {
            Request = request;
            Lang = lang;
        }
    }
}
