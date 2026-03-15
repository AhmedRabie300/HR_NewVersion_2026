using Application.UARbac.Login.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Abstractions
{ 
    public interface ILoginRepository
{
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, int lang, CancellationToken ct);
        bool ValidateToken(string token);
    }
}

 