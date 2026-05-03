using Application.Common.Lookups;

namespace Application.Common.Abstractions
{
    public interface ILookupService
    {
        Task<List<LookupDto>> GetLookupAsync<T>(string? criteria = null, CancellationToken ct = default) where T : class;
    }
}