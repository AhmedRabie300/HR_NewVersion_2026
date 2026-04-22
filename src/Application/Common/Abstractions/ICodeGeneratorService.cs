using Domain.Common;
using System;

namespace Application.Common.Abstractions
{
    public interface ICodeGeneratorService
    {
        Task<string> GetNextCodeAsync<T>(CancellationToken ct = default) where T : class;

    }
}