namespace Application.Abstractions
{
    public interface ICurrentUser
    {
        int? UserId { get; }
        int CompanyId { get; }
        int Language { get; }
        bool IsAuthenticated { get; }
    }
}
