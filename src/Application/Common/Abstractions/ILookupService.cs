using Application.Common.Lookups;

namespace Application.Common.Abstractions
{
    public interface ILookupService
    {
        Task<List<LookupDto>> GetLookupAsync<T>(CancellationToken ct = default) where T : class;
    }
}