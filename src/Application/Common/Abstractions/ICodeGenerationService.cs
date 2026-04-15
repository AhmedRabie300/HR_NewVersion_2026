namespace Application.Common.Abstractions
{
    public interface ICodeGenerationService
    {
        Task<string> GenerateCodeAsync(
            int companyId,
            string? providedCode,
            Func<int, CancellationToken, Task<string?>> getMaxCodeAsync,
            Func<string, CancellationToken, Task<bool>> codeExistsAsync,
            CancellationToken cancellationToken = default);
    }
}